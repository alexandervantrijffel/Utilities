using System;

namespace Structura.SharedComponents.Utilities.ValueObjects
{
    public class PeriodException : Exception
    {
        public PeriodException(string message)
            : base(message)
        {
        }

        public PeriodException()
        {
        }
    }

    public class Period : ValueObjectBase<Period>
    {
        private readonly DateTime _end;
        private readonly DateTime _start;

        public Period(DateTime start, DateTime end)
        {
#if DEBUG
            Check.Require<PeriodException>(end >= start, "In a period, the end time must be later than the start time");
            Check.Require<PeriodException>(start != DateTime.MaxValue, "In a period, the start time cannot be MaxValue");
            Check.Require<PeriodException>(end != DateTime.MinValue, "In a period, the end time cannot be MinValue");
            // enable this one only in DEBUG mode, because services are created with a time of 0:00
            // Check.Require<PeriodException>(end != start, "In a period, the start time cannot be equal to the end time");
#endif
            _start = start;
            _end = end;
        }

        public TimeSpan Duration
        {
            get { return _end - _start; }
        }

        public DateTime Start
        {
            get { return _start; }
        }

        public DateTime End
        {
            get { return _end; }
        }

        public bool Contains(DateTime time)
        {
            // we need to be careful here with the seconds
            // a period 8:00 - 10:00 includes the second 10:00:00 but it does not overlap with period 10:00 - 12:00

            // therefore less than at the end
            return time >= _start && time < _end;
        }

        public bool Contains(TimeSpan time)
        {
            TimeSpan startTime = _start.TimeOfDay;
            TimeSpan endTime = _end.TimeOfDay;
            // less than at the end
            return time >= startTime && time < endTime;
        }

        public bool OverlapsWith(Period otherPeriod)
        {
            Period earliest = otherPeriod.Start < _start ? otherPeriod : this;
            Period latest = earliest == this ? otherPeriod : this;

            return earliest.Contains(latest.Start);
        }

        public Period Add(Period otherPeriod)
        {
            if (Contains(otherPeriod.Start))
            {
                if (otherPeriod.End > End)
                    return new Period(Start, otherPeriod.End);
            }
            else if (Contains(otherPeriod.End))
            {
                if (otherPeriod.Start < Start)
                    return new Period(otherPeriod.Start, End);
            }
            else if (otherPeriod.End == Start)
            {
                return new Period(otherPeriod.Start, End);
            }
            else if (otherPeriod.Start == End)
            {
                return new Period(Start, otherPeriod.End);
            }
            else
            {
                // other period must be continguous with this period
                throw new PeriodException("Cannot add a period that does not overlap, or connects to this period.");
            }

            return this;
        }

        public Period Remove(Period otherPeriod)
        {
            if (otherPeriod.Start == Start && otherPeriod.End == End)
            {
                throw new PeriodException(
                    "Period.Remove cannot remove otherPeriod because the otherPeriod is identical to this period.");
            }
            if (!OverlapsWith(otherPeriod))
            {
                throw new PeriodException("Period.Remove cannot remove otherPeriod because the periods do not overlap.");
            }
            if (Contains(otherPeriod.Start) && Contains(otherPeriod.End) && Start != otherPeriod.Start &&
                End != otherPeriod.End)
            {
                throw new PeriodException(
                    "Period.Remove cannot remove otherPeriod because start and end time overlap with this period. Removing the otherPeriod would result in two remaining periods.");
            }
            if (Contains(otherPeriod.Start) && Start != otherPeriod.Start)
            {
                // remove the last part of this period
                return new Period(otherPeriod.Start, End);
            }
            else
            {
                // remove the first part of this period
                return new Period(Start, otherPeriod.End);
            }
        }

        public bool HasExpired()
        {
            return DateTime.UtcNow > _end;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1}", _start, _end);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public static bool operator ==(Period a, Period b)
        {
            return OperatorEquals(a, b);
        }

        public static bool operator !=(Period a, Period b)
        {
            return !OperatorEquals(a, b);
        }

        /// <summary>
        /// Converts the Start UTC and End UTC date times to date times of the given time zone id
        /// </summary>
        /// <param name="localTimeZoneId"></param>
        /// <returns></returns>
        public Period ToLocalPeriod(string localTimeZoneId)
        {
            return new Period(Start.ToLocalTime(localTimeZoneId), End.ToLocalTime(localTimeZoneId));
        }
    }
}