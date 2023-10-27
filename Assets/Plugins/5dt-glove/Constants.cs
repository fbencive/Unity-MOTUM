namespace Glove5dt
{
    public static class Sensors
    {
        public const int THUMB_NEAR = 0;
        public const int THUMB_FAR = 1;
        public const int THUMB_INDEX = 2;
        public const int INDEX_NEAR = 3;
        public const int INDEX_FAR = 4;
        public const int INDEX_MIDDLE = 5;
        public const int MIDDLE_NEAR = 6;
        public const int MIDDLE_FAR = 7;
        public const int MIDDLE_RING = 8;
        public const int RING_NEAR = 9;
        public const int RING_FAR = 10;
        public const int RING_LITTLE = 11;
        public const int LITTLE_NEAR = 12;
        public const int LITTLE_FAR = 13;
    };

    public enum Hand
    {
        LEFT = 1,
        RIGHT = -1
    };

    public enum Calibration
    {
        FIST = 1,
        OPEN = 2,
        FLAT = 3,
        THUMB = 4
    }
}