namespace Glove5dt
{
    public class MovingAverage
    {
        readonly public uint NumChannels = 20;
        private uint width = 3;
        private float[][] rawValues;
        private float[] averagedValues;
        private uint readCount = 0;
        private uint readIndex = 0;

        public MovingAverage(uint width = 3)
        {
            averagedValues = new float[NumChannels];
            Width = width;
        }

        public uint Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                rawValues = new float[width][];
                readCount = 0;
                readIndex = 0;
            }
        }

        public float[] addSample(float[] values)
        {
            rawValues[readIndex] = values;
            readCount++;
            readIndex = (readIndex + 1) % Width;

            if (readCount >= Width)
            {
                float[] meanValue = new float[NumChannels];
                for (int i = 0; i < NumChannels; i++)
                {
                    averagedValues[i] = 0;
                    for (int j = 0; j < Width; j++)
                        averagedValues[i] += rawValues[j][i];
                    averagedValues[i] = averagedValues[i] / Width;
                }
            }
            return averagedValues;
        }
    }
}
