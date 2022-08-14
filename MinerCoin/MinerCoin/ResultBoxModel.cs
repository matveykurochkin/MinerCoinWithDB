using System.ComponentModel;

namespace MinerCoin
{
    internal class ResultBoxModel : INotifyPropertyChanged
    {
        private long _result;

        public long Result
        {
            get { return _result; }
            set
            {
                _result = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(nameof(Result)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
