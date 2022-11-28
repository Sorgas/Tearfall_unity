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
        public const int ticksPerHour = 1200; // ticks of hour (hour is 60 minutes)
        public const int ticksPerDay = 28800; // ticks of day (day is 24 hours)

        public const float baseRestSpeed = 1;

        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public int tick;

        public void update() {
            tick++;
            if (tick == ticksPerMinute) {
                tick = 0;
                minute++;
                if (minute == 60) {
                    minute = 0;
                    hour++;
                    if(hour == 24) {
                        hour = 0;
                        day++;
                        if(day == 31) {
                            day = 1;
                            month ++;
                            if(month == 13) {
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