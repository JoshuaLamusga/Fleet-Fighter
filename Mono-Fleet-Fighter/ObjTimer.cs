namespace FleetFighter
{
    /// <summary>
    /// Draws the titles and controls the menus
    /// </summary>
    public class ObjTimer
    {
        public int ticks, secs, mins;
        public int ticksStart, secsStart, minsStart;
        public bool isCountingDown, atZero = false;

        ///<summary>
        ///The timer will count down from the given seconds and minutes.
        ///</summary>
        public ObjTimer(int ticks, int secs, int mins)
        {
            isCountingDown = true;
            this.ticks = ticks;
            this.secs = secs;
            this.mins = mins;
            ticksStart = ticks;
            secsStart = secs;
            minsStart = mins;
        }

        ///<summary>
        ///Creates a timer counting up.
        ///</summary>
        public ObjTimer()
        {
            isCountingDown = false;
            ticks = 0;
            secs = 0;
            mins = 0;
            ticksStart = ticks;
            secsStart = secs;
            minsStart = mins;
        }

        /// <summary>
        /// Returns the current amount of ticks.
        /// </summary>
        public int GetTicks()
        {
            return ticks;
        }

        /// <summary>
        /// Returns the current amount of seconds.
        /// </summary>
        public int GetSecs()
        {
            return secs;
        }

        /// <summary>
        /// Returns the current amount of minutes.
        /// </summary>
        public int GetMins()
        {
            return mins;
        }

        /// <summary>
        /// Returns whether or not the time matches the specified time.
        /// </summary>
        public bool AtTime(int ticks, int secs, int mins)
        {
            if (this.ticks == ticks &&
                this.secs == secs &&
                this.mins == mins)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Resets the time.
        /// </summary>
        public void Reset()
        {
            ticks = ticksStart;
            secs = secsStart;
            mins = minsStart;
        }

        /// <summary>
        /// Call this in the main update loop.
        /// </summary>
        public void Update()
        {
            if (!isCountingDown)
            {
                //Counts up the time
                ticks++;
                if (secs >= 60)
                {
                    secs = 0;
                    mins++;
                }
                if (ticks >= 60)
                {
                    ticks = 0;
                    secs++;
                }
            }
            else
            {
                //Counts down the time
                if (mins > 0 || secs > 0 || ticks > 0)
                {
                    ticks--;
                    if (secs < 0)
                    {
                        secs = 60;
                        mins--;
                    }
                    if (ticks < 0)
                    {
                        ticks = 60;
                        secs--;
                    }
                }
                else
                {
                    atZero = true;
                }
            }
        }
    }
}