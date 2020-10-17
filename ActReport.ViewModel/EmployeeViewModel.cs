using ActReport.Core.Entities;
using ActReport.Persistence;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;

namespace ActReport.ViewModel
{
    public class EmployeeViewModel : BaseViewModel
    {
        private string _firstName;
        private string _lastName;
        private string _filterText;
        private CollectionView _view;
        private Employee _selectedEmployee;
        private Employee _newEmployee;
        private ObservableCollection<Employee> _employees;
        private ICommand _cmdSaveChanges;
        private ICommand _cmdAddEntries;

        public ICommand CmdSaveChanges
        {
            get
            {
                if(_cmdSaveChanges == null)
                {
                    _cmdSaveChanges = new RelayCommand(
                        execute: _ =>
                        {
                            using UnitOfWork uow = new UnitOfWork();
                            _selectedEmployee.FirstName = _firstName;
                            _selectedEmployee.LastName = _lastName;
                            uow.EmployeeRepository.Update(_selectedEmployee);
                            uow.Save();

                            LoadEmployees();
                        },
                        canExecute: _ => _selectedEmployee != null && LastName.Length >= 3);
                }

                return _cmdSaveChanges;
            }
            set { _cmdSaveChanges = value; }
        }

        public ICommand CmdAddEntries
        {
            get
            {
                if (_cmdAddEntries == null)
                {
                    _cmdAddEntries = new RelayCommand(
                        execute: _ =>
                        {
                            using UnitOfWork uow = new UnitOfWork();
                            _newEmployee = new Employee
                            {
                                FirstName = _firstName,
                                LastName = _lastName
                            };
                            uow.EmployeeRepository.Insert(_newEmployee);
                            uow.Save();

                            LoadEmployees();
                        },
                        canExecute: _ => !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(FirstName) &&
                                        LastName.Length >= 3 && FirstName.Length >= 3);
                }

                return _cmdAddEntries;
            }
            set { _cmdAddEntries = value; }
        }

        public EmployeeViewModel()
        {
            LoadEmployees();
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Employees);
            _view.Filter = EmployeeFilter;
        }

        private void LoadEmployees()
        {
            using(UnitOfWork uow = new UnitOfWork())
            {
                var employees = uow.EmployeeRepository
                    .Get(
                        orderBy:
                            coll => coll.OrderBy(emp => emp.LastName))
                    .ToList();

                Employees = new ObservableCollection<Employee>(employees);
            }
        }

        public bool EmployeeFilter(object item)
        {
            if(String.IsNullOrEmpty(FilterText))
            {
                return true;
            }
            else
            {
                return ((item as Employee).LastName.StartsWith(FilterText, StringComparison.OrdinalIgnoreCase));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                FirstName = _selectedEmployee?.FirstName;
                LastName = _selectedEmployee?.LastName;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }

        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                _view.Refresh();
                OnPropertyChanged(nameof(FilterText));
            }
        }

        public Employee NewEmployee
        {
            get => _newEmployee;
            set
            {
                _newEmployee = value;
                FirstName = _newEmployee?.FirstName;
                LastName = _newEmployee?.LastName;
                OnPropertyChanged(nameof(NewEmployee));
            }
        }

        public ObservableCollection<Employee> Employees
        {
            get => _employees;
            set
            {
                _employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }
    }
}
