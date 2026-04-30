namespace KooliProjekt.WpfApplication
{
    public class Project : NotifyPropertyChangedBase
    {
        private int _id;
        private string _title;
        private DateTime _startDate;
        private DateTime _dueDate;
        private decimal _budget;
        private decimal _pricePerHour;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime DueDate
        {
            get
            {
                return _dueDate;
            }
            set
            {
                _dueDate = value;
                NotifyPropertyChanged();
            }
        }

        public decimal Budget
        {
            get
            {
                return _budget;
            }
            set
            {
                _budget = value;
                NotifyPropertyChanged();
            }
        }

        public decimal PricePerHour
        {
            get
            {
                return _pricePerHour;
            }
            set
            {
                _pricePerHour = value;
                NotifyPropertyChanged();
            }
        }

        public string Name
        {
            get
            {   
                return _title;
            }
            set
            {
                _title = value;
                NotifyPropertyChanged();
            }
        }
    }
}
