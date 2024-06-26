namespace game.model.system {
    /**
     * Rolls time for game.
     * {@link Timer} provides update calls with specific interval ({@link GameTime#gameSpeed}).
     * Then, states {@link TimeUnitState}s updated(clock thing).
     * Largest updated time unit is passed to {@link GameModel} to update systems.
     * After day is over, {@link WorldCalendar} is notified to change game date.
     *
     * @author Alexander on 07.10.2018.
     */
    // stores game time state and constants
    public class GameTime {
        public const int ticksPerMinute = 20; // ticks of minute
        public const int ticksPerHour = ticksPerMinute * 60; // ticks of hour (hour is 60 minutes) 1200
        public const int ticksPerDay = ticksPerHour * 24; // ticks of day (day is 24 hours) 28800

        public const float baseRestSpeed = 1;

        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int tick;

        public void update(int ticks) {
            for (int i = 0; i < ticks; i++) {
                tick++;
                if (tick >= ticksPerMinute) {
                    tick -= ticksPerMinute;
                    minute++;
                    if (minute == 60) {
                        minute = 0;
                        hour++;
                        if (hour == 24) {
                            hour = 0;
                            day++;
                            if (day == 31) {
                                day = 1;
                                month++;
                                if (month == 13) {
                                    month = 1;
                                    year++;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}