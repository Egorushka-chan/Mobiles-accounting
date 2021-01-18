using Mobiles_accounting.Model;
using Mobiles_accounting.Views;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Mobiles_accounting
{
    class ViewModel : BaseVM
    {
        public static string ConnectionInfo = "server = localhost;port = 3306;username=root;password=tamara23;database=mobiles accounting";

        private List<string> QueriesToBeExecuted = new List<string>();

        public int EntranseID { get; set; }

        public ObservableCollection<Note> History { get; set; }

        public ObservableCollection<Program> Programs { get; set; } // Коллекции записей
        public ObservableCollection<Check> Checks { get; set; }
        public ObservableCollection<Developer> Developers { get; set; }

        public ICollectionView ProgramsView { get; set; } // Фильрация

        private Program selectedProgram;
        public Program SelectedProgram
        {
            get => selectedProgram;
            set
            {
                selectedProgram = value;
                OnPropertyChanged(nameof(SelectedProgram));
            }
        }

        private Check selectedCheck;
        public Check SelectedCheck
        {
            get => selectedCheck;
            set
            {
                selectedCheck = value;
                OnPropertyChanged(nameof(SelectedCheck));
            }
        }

        private Developer selectedDeveloper;
        public Developer SelectedDeveloper
        {
            get => selectedDeveloper;
            set
            {
                selectedDeveloper = value;
                OnPropertyChanged(nameof(SelectedDeveloper));
            }
        }

        

        private int idAddInProgram; // Поля окна AddWindow
        private string nameAddInProgram;
        private Developer developerAddInProgram;
         
        //private State stateAddEquipment;

        private int idAddInCheckDeveloper; // Поля окон AddStateWindow и AddSubdivisionWindow
        private string nameAddInCheckDeveloper;
        private Program programAddInCheck;
        //private string noteAddStateSubdivision;

        private bool isNofificationsOn = false;

        public int IDAddInCheckDeleloper
        {
            get => idAddInCheckDeveloper;
            set
            {
                idAddInCheckDeveloper = Math.Abs(value);
                OnPropertyChanged(nameof(IDAddInCheckDeleloper));
            }
        }


        public Program ProgramAddInCheck
        {
            get => programAddInCheck;
            set
            {
                programAddInCheck = value;
                OnPropertyChanged(nameof(ProgramAddInCheck));
            }
        }

        public string NameAddInCheckDeveloper
        {
            get => nameAddInCheckDeveloper;
            set
            {
                nameAddInCheckDeveloper = value;
                OnPropertyChanged(nameof(NameAddInCheckDeveloper));
            }
        }

        //public string NoteAddStateSubdivision
        //{
        //    get => noteAddStateSubdivision;
        //    set
        //    {
        //        noteAddStateSubdivision = value;
        //        OnPropertyChanged(nameof(NoteAddStateSubdivision));
        //    }
        //}

        public int IDAddInProgram // tb1 окон AddWindow и ReplaceWindow
        {
            get => idAddInProgram;
            set
            {
                idAddInProgram = Math.Abs(value);
                OnPropertyChanged(nameof(IDAddInProgram));
            }
        }

        public string NameInAddProgram // tb2 окон AddWindow и ReplaceWindow
        {
            get => nameAddInProgram;
            set
            {

                nameAddInProgram = value;
                OnPropertyChanged(nameof(NameInAddProgram));
            }
        }


        public Developer DeveloperAddInProgram // tb3 окон AddWindow и ReplaceWindow
        {
            get => developerAddInProgram;
            set
            {
                developerAddInProgram = value;
                OnPropertyChanged("SubdivisionAddEquipment");
            }
        }

        //public State StateAddEquipment   // tb4 окон AddWindow и ReplaceWindow
        //{
        //    get => stateAddEquipment;
        //    set
        //    {
        //        stateAddEquipment = value;
        //        OnPropertyChanged("StateAddEquipment");
        //    }
        //}

        public bool IsNotificationsOn // CheckBox "Уведомлять о изменениях"
        {
            get => isNofificationsOn;
            set
            {
                isNofificationsOn = value;
                OnPropertyChanged("IsNotificationsOn");
            }
        }

        

        private MyCommand openCommand;
        public MyCommand OpenCommand // Команда для автозаполнения данных в окнах ReplaceWindow и AddWindow
        {
            get
            {
                return openCommand ??
                  (openCommand = new MyCommand(obj =>
                  {
                      string str = obj as string;
                      if (str == "AddEq" || str == "ReplaceEq")
                      {
                          if (SelectedProgram != null)
                          {
                              if (str == "AddEq")
                                  IDAddInProgram = Programs.Count + 1;
                              else if (str == "ReplaceEq")
                                  IDAddInProgram = SelectedProgram.ID;
                              NameInAddProgram = SelectedProgram.Name;
                              DeveloperAddInProgram = SelectedProgram.Developer;
                          }
                      }
                      else if (str == "AddSt")
                      {
                          IDAddInCheckDeleloper = Checks.Count + 1;
                          NameAddInCheckDeveloper = SelectedCheck.UserName;
                          ProgramAddInCheck = SelectedCheck.Program;
                      }
                      else if (str == "AddSub")
                      {
                          IDAddInCheckDeleloper = Developers.Count + 1;
                          NameAddInCheckDeveloper = SelectedDeveloper.Name;
                      }
                  }));
            }
        }

        private MyCommand addProgramCommand;
        public MyCommand AddProgramCommand // Команда добавления новой записи из окна AddWindow
             => addProgramCommand ??
                    (addProgramCommand = new MyCommand(obj =>
                    {
                        Program program = new Program()
                        {
                            ID = IDAddInProgram,
                            Name = NameInAddProgram,
                            Developer = developerAddInProgram
                        };


                        bool IsINUnique = true;
                        foreach (Program programForeach in Programs)
                            if (programForeach.ID == program.ID)
                                IsINUnique = false;

                        if (IsFieldsNotNull(program) & IsINUnique)
                        {
                            Programs.Insert(Programs.Count, program);
                            SelectedProgram = program;
                            QueriesToBeExecuted.Add(
                                $"INSERT INTO program VALUES " +
                                $"({program.ID}," + // IN
                                $"'{program.Name}', " + // Name
                                $"{program.Developer.ID})");
                            NewHistoryNote($"Добавлена новая программа - {program.ID}. {program.Name}");
                        }
                        else
                            MessageBox.Show("Неправильные данные");
                    }));

        private MyCommand addCheckCommand;
        public MyCommand AddCheckCommand
        {
            get
            {
                return addCheckCommand ??
                  (addCheckCommand = new MyCommand(obj =>
                  {
                      Check check = new Check
                      {
                          ID = IDAddInCheckDeleloper,
                          UserName = NameAddInCheckDeveloper,
                          Date = DateTime.Now,
                          Program = ProgramAddInCheck
                      };

                      bool IsINUnique = true;
                      foreach (Check checkForeach in Checks)
                          if (checkForeach.ID == check.ID)
                              IsINUnique = false;

                      if (IsINUnique && check.UserName != default)
                      {
                          Checks.Insert(Checks.Count, check);
                          QueriesToBeExecuted.Add(
                              "INSERT INTO check VALUES " +
                              $"({check.ID}, " +
                              $"'{check.UserName}', " +
                              $"'{check.Date:yyyy-MM-dd H:mm:ss}', " +
                              $"'{check.Program.ID}')");
                          NewHistoryNote($"Добавлен новый чек - {check.ID}. {check.Program.Name}");
                      }
                      else
                          MessageBox.Show("Неправильные данные");
                  }));
            }
        }

        private MyCommand addDeveloperCommand;
        public MyCommand AddDeveloperCommand
        {
            get
            {
                return addDeveloperCommand ??
                  (addDeveloperCommand = new MyCommand(obj =>
                  {
                      Developer developer = new Developer
                      {
                          ID = IDAddInCheckDeleloper,
                          Name = NameAddInCheckDeveloper
                      };

                      bool IsINUnique = true;
                      foreach (Developer developerForeach in Developers)
                          if (developerForeach.ID == developer.ID)
                              IsINUnique = false;

                      if (IsINUnique && developer.Name != default)
                      {
                          Developers.Insert(Developers.Count, developer);
                          QueriesToBeExecuted.Add(
                              "INSERT INTO developer VALUES " +
                              $"({developer.ID}, " +
                              $"'{developer.Name}')");
                          NewHistoryNote($"Добавлен новый разработчик - {developer.ID}. {developer.Name}");
                      }
                      else
                          MessageBox.Show("Неправильные данные");
                  }));
            }
        }

        private MyCommand replaceCommand;
        public MyCommand ReplaceCommand
        {
            get
            {
                return replaceCommand ??
                  (replaceCommand = new MyCommand(obj =>
                  {
                      var oldProgram = SelectedProgram;
                      Program program = new Program()
                      {
                          ID = IDAddInProgram,
                          Name = NameInAddProgram,
                          Developer = developerAddInProgram
                      };

                      bool IsIDOkay = true;
                      foreach (Program programForeach in Programs)
                          if (programForeach.ID == program.ID)
                              if (SelectedProgram.ID != program.ID)
                                  IsIDOkay = false;

                      if (IsFieldsNotNull(program) & IsIDOkay)
                      {
                          Programs[Programs.IndexOf(SelectedProgram)] = program;
                          QueriesToBeExecuted.Add(
                              $"UPDATE `program` " +
                              $"SET `Name` = '{program.Name}', " +
                              $"`Developer ID` = {program.Developer.ID} " +
                              $"Where `ID` = {program.ID}");
                          NewHistoryNote($"Программа '{oldProgram.ID}. {oldProgram.Name}' заменена на '{program.ID}. {program.Name}'");
                      }
                      else
                          MessageBox.Show("Неправильные данные");
                  }, obj => Programs.Count > 0));
            }
        }

        bool IsFieldsNotNull(Program program)
        {
            bool[] IsFieldNotNull = new bool[4];

            IsFieldNotNull[0] = program.ID != default;
            IsFieldNotNull[1] = program.Name != default;
            IsFieldNotNull[2] = program.Developer.Name != default;
            IsFieldNotNull[3] = program.Developer.ID != default;
            bool IsFieldsNotNull = true;
            foreach (bool entity in IsFieldNotNull)
                if (!entity)
                    IsFieldsNotNull = false;

            return IsFieldsNotNull;
        }


        private MyCommand deleteProgramCommand;
        public MyCommand DeleteProgramCommand // Удаление записи из Equipment
        {
            get
            {
                return deleteProgramCommand ??
                  (deleteProgramCommand = new MyCommand(obj =>
                  {
                      var oldProgram = SelectedProgram;
                      var result = MessageBox.Show($"Вы уверены, что хотите удалить ({SelectedProgram.ID}, '{SelectedProgram.Name}', {SelectedProgram.Developer.Name}) ?",
                          "Проверка", MessageBoxButton.YesNo);
                      if (result == MessageBoxResult.Yes)
                      {
                          QueriesToBeExecuted.Add($"DELETE FROM `program` WHERE `ID` = {SelectedProgram.ID}");
                          Programs.Remove(SelectedProgram);
                          if (Programs.Count > 0)
                              SelectedProgram = Programs[0];
                          else SelectedProgram = null;
                          NewHistoryNote($"Программа удалена - {oldProgram.ID}. {oldProgram.Name}");
                      }
                      
                  }, obj => SelectedProgram != null));
            }
        }

        private MyCommand deleteCheckCommand;
        public MyCommand DeleteCheckCommand
        {
            get
            {
                return deleteCheckCommand ??
                  (deleteCheckCommand = new MyCommand(obj =>
                  {
                      var oldCheck = SelectedCheck;
                      var result = MessageBox.Show($"Вы уверены, что хотите удалить ({SelectedCheck.ID}, '{SelectedCheck.UserName}', '{SelectedCheck.Date}', '{SelectedCheck.Program.Name}') ?",
                          "Проверка", MessageBoxButton.YesNo);
                      if (result == MessageBoxResult.Yes)
                      {
                          QueriesToBeExecuted.Add($"DELETE FROM `check` WHERE `ID` = {SelectedCheck.ID}");
                          Checks.Remove(SelectedCheck);
                          SelectedCheck = Checks[0];
                          NewHistoryNote($"Чек удален - {oldCheck.ID}. {oldCheck.Program.Name}");
                      }

                      else
                          MessageBox.Show("Это состояние используется в данный момент и не может быть удалено");
                  }, obj => SelectedCheck != null && Checks.Count > 0));
            }
        }

        private MyCommand deleteSubdivisionCommand;
        public MyCommand DeleteSubdivisionCommand
        {
            get
            {
                return deleteSubdivisionCommand ??
                  (deleteSubdivisionCommand = new MyCommand(obj =>
                  {
                      var oldDeveloper = SelectedDeveloper;
                      bool IsSubdivisionNotUsing = true;
                      foreach (Program program in Programs)
                          if (SelectedDeveloper.ID == program.Developer.ID)
                              IsSubdivisionNotUsing = false;

                      if (IsSubdivisionNotUsing)
                      {
                          var result = MessageBox.Show($"Вы уверены, что хотите удалить ({SelectedDeveloper.ID}, '{SelectedDeveloper.Name}') ?",
                          "Проверка", MessageBoxButton.YesNo);
                          if (result == MessageBoxResult.Yes)
                          {
                              QueriesToBeExecuted.Add($"DELETE FROM `developer` WHERE `ID` = {SelectedDeveloper.ID}");
                              Developers.Remove(SelectedDeveloper);
                              SelectedDeveloper = Developers[0];
                              NewHistoryNote($"Разработчик удален - {oldDeveloper.ID}. {oldDeveloper.Name}");
                          }
                      }
                      else
                          MessageBox.Show("Это подразделение используется в данный момент и не может быть удалено");
                  }, obj => SelectedDeveloper != null && Developers.Count > 0));
            }
        }

        private MyCommand saveChangesCommand;
        public MyCommand SaveChangesCommand // Удаление записи из Equipment
        {
            get
            {
                return saveChangesCommand ??
                  (saveChangesCommand = new MyCommand(obj =>
                  {
                      bool IsExecutingStable = true;
                      string queries = Environment.NewLine;
                      foreach (string query in QueriesToBeExecuted)
                          queries += query + Environment.NewLine + Environment.NewLine;
                      if (MessageBox.Show($"Будут выполнены следующие запросы: {queries}", "Выполнить?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                      {
                          foreach (string query in QueriesToBeExecuted)
                          {
                              try
                              {
                                  ExecuteDataQuery(query, "Тут не выводится таблицы, так что все равно");
                              }
                              catch (Exception e)
                              {
                                  MessageBox.Show($"Провалено выполенение: {Environment.NewLine}{query}{Environment.NewLine}Ошибка: {e.Message}");
                                  IsExecutingStable = false;
                              }
                          }
                          QueriesToBeExecuted.Clear();
                          if (!IsExecutingStable)
                          {
                              InitializeCollections();
                          }
                      }
                  }, obj => QueriesToBeExecuted.Count > 0));
            }
        }

        private MyCommand cancelChangesCommand;
        public MyCommand CancelChangesCommand
        {
            get
            {
                return cancelChangesCommand ??
                  (cancelChangesCommand = new MyCommand(obj =>
                  {
                      if (MessageBox.Show("Отменить изменения?", "Вопрос", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                      {
                          bool Is = IsNotificationsOn;
                          IsNotificationsOn = false;
                          QueriesToBeExecuted.Clear();
                          InitializeCollections();
                          IsNotificationsOn = Is;
                      }
                  }, obj => QueriesToBeExecuted.Count > 0));
            }
        }

        public ViewModel() // Констуктор (как не странно)
        {
            Checks = new ObservableCollection<Check>();
            Developers = new ObservableCollection<Developer>();
            Programs = new ObservableCollection<Program>();
            History = new ObservableCollection<Note>();

            var entranseTable = ExecuteDataQuery("Select (Max(Entrance_num) + 1) AS Entr from history");
            var row = entranseTable.Rows[0];
            EntranseID = Convert.ToInt32(row["Entr"]);

            InitializeCollections();
            ProgramsView = CollectionViewSource.GetDefaultView(Programs);

            Programs.CollectionChanged += Programs_CollectionChanged;
            Checks.CollectionChanged += Checks_CollectionChanged;
            Developers.CollectionChanged += Developers_CollectionChanged;

            NewHistoryNote("Вход");
            foreach (string query in QueriesToBeExecuted)
            {
                try
                {
                    ExecuteDataQuery(query, "Тут не выводится таблицы, так что все равно");
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Провалено выполенение: {Environment.NewLine}{query}{Environment.NewLine}Ошибка: {e.Message}");
                }
            }
            QueriesToBeExecuted.Clear();

            foreach (Program program in Programs)
                program.Quantity = Checks.Where(u => u.Program == program).Count();

            ReportTable = new DataTable();
            ReportFilters = new List<ReportFilter>();
        }

        private void InitializeCollections() // Заполнение коллекций данных
        {
            DataSet dataSet = new DataSet();

            dataSet.Tables.Add(ExecuteDataQuery("Select `program`.`ID` AS `ID`, `program`.`Name` AS `Name`, `program`.`Developer ID` AS `Developer` " +
                "from `program` JOIN `developer` ON `program`.`Developer ID` = `developer`.`ID` " +
                "order by `ID`",
                "Program"));

            dataSet.Tables.Add(ExecuteDataQuery("select `check`.`ID` AS ID, `check`.`Username` AS UserName, `check`.`Date` AS Date, `check`.`Program ID` AS Program from `check`",
                "Check"));

            dataSet.Tables.Add(ExecuteDataQuery("select db.ID AS ID, db.Name AS Name from developer db",
                "Developer"));

            dataSet.Tables.Add(ExecuteDataQuery("select hs.`Entrance_num` AS EntranceNum, hs.`Action ID` AS ActionID, hs.Date AS Date, hs.Action AS Action from history hs",
                "History"));



            Developers.Clear();
            foreach (DataRow dataRow in dataSet.Tables["Developer"].Rows)
            {
                Developers.Add(new Developer
                {
                    ID = (int)dataRow["ID"],
                    Name = (string)dataRow["Name"]
                });
            }

            Programs.Clear();
            foreach (DataRow dataRow in dataSet.Tables["Program"].Rows)
            {
                foreach (Developer developer in Developers)
                {
                    if ((int)dataRow["Developer"] == developer.ID)
                        Programs.Add(new Program
                        {
                            ID = (int)dataRow["ID"],
                            Name = (string)dataRow["Name"],
                            Developer = developer
                        });
                }
            }

            Checks.Clear();
            foreach (DataRow dataRow in dataSet.Tables["Check"].Rows)
            {
                foreach (Program program in Programs)
                {
                    if((int)dataRow["Program"] == program.ID)
                    {
                        Checks.Add(new Check
                        {
                            ID = (int)dataRow["ID"],
                            UserName = (string)dataRow["UserName"],
                            Date = (DateTime)dataRow["Date"],
                            Program = program
                        });
                    }
                }
            }

            foreach (DataRow dataRow in dataSet.Tables["History"].Rows)
            {
                History.Add(new Note
                {
                    EntranceNum = (int)dataRow["EntranceNum"],
                    ActionID = (int)dataRow["ActionID"],
                    Date = (DateTime)dataRow["Date"],
                    Action = (string)dataRow["Action"]
                });
            }

            

            if (Programs.Count != 0) SelectedProgram = Programs[0];
            if (Checks.Count != 0) SelectedCheck = Checks[0];
            if (Developers.Count != 0) SelectedDeveloper = Developers[0];
        }

        private void Programs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsNotificationsOn)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Program program = e.NewItems[0] as Program;
                        MessageBox.Show($"Добавлена программа: ({program.ID}, {program.Name}, {program.Developer.Name})");
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        Program replacedProgram = e.OldItems[0] as Program;
                        Program replacingProgram = e.NewItems[0] as Program;
                        MessageBox.Show($"({replacedProgram.ID}, {replacedProgram.Name}, {replacedProgram.Developer.Name}) " +
                            $"заменен на " +
                            $"({replacingProgram.ID}, {replacingProgram.Name}, {replacingProgram.Developer.Name})");
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        Program removedProgram = e.OldItems[0] as Program;
                        MessageBox.Show($"Удалена программа: {removedProgram.ID} {removedProgram.Name} {removedProgram.Developer.Name}");
                        break;
                }
            }
        }

        private void Developers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsNotificationsOn)
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        Developer developer = e.NewItems[0] as Developer;
                        MessageBox.Show($"Добавлен разработчик : ({developer.ID}, {developer.Name})");
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        Developer removedDeveloper = e.OldItems[0] as Developer;
                        MessageBox.Show($"Удален разработчик: ({removedDeveloper.ID}, {removedDeveloper.Name})");
                        break;
                }
            }
        }

        private void Checks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (IsNotificationsOn)
                    {
                        Check check = e.NewItems[0] as Check;
                        MessageBox.Show($"Добавлено состояние : ({check.ID}, {check.UserName}, {check.Program.Name})");
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (IsNotificationsOn)
                    {
                        Check removedCheck = e.OldItems[0] as Check;
                        MessageBox.Show($"Удалено снаряжение: ({removedCheck.ID}, {removedCheck.UserName}, {removedCheck.Program.Name})");
                    }
                    break;
            }
            
            foreach (Program program in Programs)
                program.Quantity = Checks.Where(u => u.Program == program).Count();
        }

        /// <summary>
        /// Выполняет запрос к базе данных
        /// </summary>
        /// <returns>
        /// Если запрос не предпологает возращения данных, выходное значение можно не принимать
        /// </returns>
        private DataTable ExecuteDataQuery(string CommandText, string TableName = "None")
        {
            DataTable dataTable = new DataTable(TableName);
            MySqlConnection connection = new MySqlConnection(ConnectionInfo);
            MySqlCommand command = new MySqlCommand(CommandText, connection);

            connection.Open();
            dataTable.Load(command.ExecuteReader());
            connection.Close();

            return dataTable;
        }

        void NewHistoryNote(string Info)
        {
            Note note = new Note
            {
                EntranceNum = EntranseID,
                ActionID = History.Where(u => u.EntranceNum == EntranseID).Count() + 1,
                Date = DateTime.Now,
                Action = Info
            };

            History.Add(note);

            QueriesToBeExecuted.Add(
                $"Insert into history " +
                $"Values ({note.EntranceNum},{note.ActionID},'{note.Date:yyyy-MM-dd H:mm:ss}','{note.Action}')");
        }

        #region Отчеты

        private DataTable _ReportTable;
        public DataTable ReportTable
        {
            get => _ReportTable;
            set
            {
                _ReportTable = value;
                OnPropertyChanged(nameof(ReportTable));
            }
        }

        private string _LogText;
        public string LogText
        {
            get => _LogText;
            set
            {
                _LogText = value;
                OnPropertyChanged(nameof(LogText));
            }
        }

        private List<ReportFilter> _ReportFilters;
        public List<ReportFilter> ReportFilters
        { 
            get => _ReportFilters; 
            set { 
                _ReportFilters = value; 
                OnPropertyChanged(nameof(ReportFilters));
            } 
        }

        private MyCommand programFilterCommand;
        public MyCommand ProgramFilterCommand
        {
            get
            {
                return programFilterCommand ??
                  (programFilterCommand = new MyCommand(obj =>
                  {
                      LogText += "Выполнение параметрического запроса Программы..." + Environment.NewLine;
                      ReportFilter OtId = new ReportFilter { Context = "От X ID" };
                      OtId.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Program)
                          {
                              Program prog = value as Program;
                              if (prog.ID >= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter DoID = new ReportFilter { Context = "До X ID" };
                      DoID.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Program)
                          {
                              Program prog = value as Program;
                              if (prog.ID <= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter Name = new ReportFilter { Context = "Название" };
                      Name.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Program)
                          {
                              Program prog = value as Program;
                              if (string.IsNullOrWhiteSpace(content) || content.Contains(prog.Name))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter Developer = new ReportFilter { Context = "Разработчик" };
                      Developer.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Program)
                          {
                              Program prog = value as Program;
                              if (string.IsNullOrWhiteSpace(content) || content.Contains(prog.Developer.Name))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter OtQa = new ReportFilter { Context = "От X заказов" };
                      OtQa.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Program)
                          {
                              Program prog = value as Program;
                              if (prog.Quantity >= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter DoQa = new ReportFilter { Context = "До X заказов" };
                      OtQa.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Program)
                          {
                              Program prog = value as Program;
                              if (prog.Quantity <= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilters.Add(OtId);
                      ReportFilters.Add(DoID);
                      ReportFilters.Add(Name);
                      ReportFilters.Add(Developer);
                      ReportFilters.Add(OtQa);
                      ReportFilters.Add(DoQa);

                      var parametricWindow = new ParametricWindow();
                      parametricWindow.DataContext = this;
                      parametricWindow.ShowDialog();

                      var ParamPrograms = from t in Programs
                                           where ReportFilters[0].GetResult(t) == true && ReportFilters[1].GetResult(t) == true 
                                           && ReportFilters[2].GetResult(t) == true && ReportFilters[3].GetResult(t) == true
                                           && ReportFilters[4].GetResult(t) == true && ReportFilters[5].GetResult(t) == true
                                           select t;

                      DataTable newTable = new DataTable("ParamOperators");
                      List<string> names = new List<string> { "ID", "Название", "Производитель", "Заказы"};
                      foreach (string name in names)
                          newTable.Columns.Add(new DataColumn(name));
                      foreach (Program program in ParamPrograms)
                      {
                          DataRow dataRow = newTable.NewRow();
                          dataRow["ID"] = program.ID;
                          dataRow["Название"] = program.Name;
                          dataRow["Производитель"] = program.Developer.Name;
                          dataRow["Заказы"] = program.Quantity;
                          newTable.Rows.Add(dataRow);
                      }
                      ReportTable.Dispose();
                      ReportTable = newTable;

                      LogText += $"Выполнено. Число выведенных строк - {ParamPrograms.Count()}" + Environment.NewLine;

                      ReportFilters.Clear();
                  }));
            }
        }

        private MyCommand devFilterCommand;
        public MyCommand DeveloperFilterCommand
        {
            get
            {
                return devFilterCommand ??
                  (devFilterCommand = new MyCommand(obj =>
                  {
                      LogText += "Выполнение параметрического запроса Разработчики..." + Environment.NewLine;
                      ReportFilter OtId = new ReportFilter { Context = "От X ID" };
                      OtId.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Developer)
                          {
                              Developer prog = value as Developer;
                              if (prog.ID >= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter DoID = new ReportFilter { Context = "До X ID" };
                      DoID.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Developer)
                          {
                              Developer prog = value as Developer;
                              if (prog.ID <= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter Name = new ReportFilter { Context = "Название" };
                      Name.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Developer)
                          {
                              Developer prog = value as Developer;
                              if (string.IsNullOrWhiteSpace(content) || content.Contains(prog.Name))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilters.Add(OtId);
                      ReportFilters.Add(DoID);
                      ReportFilters.Add(Name);

                      var parametricWindow = new ParametricWindow();
                      parametricWindow.DataContext = this;
                      parametricWindow.ShowDialog();

                      var ParamPrograms = from t in Developers
                                          where ReportFilters[0].GetResult(t) == true && ReportFilters[1].GetResult(t) == true && ReportFilters[2].GetResult(t) == true
                                          select t;

                      DataTable newTable = new DataTable("ParamOperators");
                      List<string> names = new List<string> { "ID", "Название", "Производитель", "Заказы" };
                      foreach (string name in names)
                          newTable.Columns.Add(new DataColumn(name));
                      foreach (Developer program in ParamPrograms)
                      {
                          DataRow dataRow = newTable.NewRow();
                          dataRow["ID"] = program.ID;
                          dataRow["Название"] = program.Name;
                          newTable.Rows.Add(dataRow);
                      }
                      ReportTable.Dispose();
                      ReportTable = newTable;

                      LogText += $"Выполнено. Число выведенных строк - {ParamPrograms.Count()}" + Environment.NewLine;

                      ReportFilters.Clear();
                  }));
            }
        }

        private MyCommand checkFilterCommand;
        public MyCommand CheckFilterCommand
        {
            get
            {
                return checkFilterCommand ??
                  (checkFilterCommand = new MyCommand(obj =>
                  {
                      LogText += "Выполнение параметрического запроса Чеки..." + Environment.NewLine;
                      ReportFilter OtId = new ReportFilter { Context = "От X ID" };
                      OtId.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Check)
                          {
                              Check prog = value as Check;
                              if (prog.ID >= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter DoID = new ReportFilter { Context = "До X ID" };
                      DoID.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Check)
                          {
                              Check prog = value as Check;
                              if (prog.ID <= Convert.ToInt32(content) || string.IsNullOrWhiteSpace(content))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter Name = new ReportFilter { Context = "Имя клиента" };
                      Name.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Check)
                          {
                              Check prog = value as Check;
                              if (string.IsNullOrWhiteSpace(content) || content.Contains(prog.UserName))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter Developer = new ReportFilter { Context = "Разработчик приложения" };
                      Developer.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Check)
                          {
                              Check prog = value as Check;
                              if (string.IsNullOrWhiteSpace(content) || content.Contains(prog.Program.Developer.Name))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilter DoQa = new ReportFilter { Context = "Приложение" };
                      DoQa.RegisterHandler(delegate (string content, object value)
                      {
                          if (value is Check)
                          {
                              Check prog = value as Check;
                              if (string.IsNullOrWhiteSpace(content) || content.Contains(prog.Program.Name))
                                  return true;
                          }
                          return false;
                      });

                      ReportFilters.Add(OtId);
                      ReportFilters.Add(DoID);
                      ReportFilters.Add(Name);
                      ReportFilters.Add(Developer);
                      ReportFilters.Add(DoQa);

                      var parametricWindow = new ParametricWindow();
                      parametricWindow.DataContext = this;
                      parametricWindow.ShowDialog();

                      var ParamChecks = from t in Checks
                                          where ReportFilters[0].GetResult(t) == true && ReportFilters[1].GetResult(t) == true
                                          && ReportFilters[2].GetResult(t) == true && ReportFilters[3].GetResult(t) == true
                                          && ReportFilters[4].GetResult(t) == true
                                          select t;

                      DataTable newTable = new DataTable("ParamOperators");
                      List<string> names = new List<string> { "ID", "ФИО", "Дата", "Программа" };
                      foreach (string name in names)
                          newTable.Columns.Add(new DataColumn(name));
                      foreach (Check Check in ParamChecks)
                      {
                          DataRow dataRow = newTable.NewRow();
                          dataRow["ID"] = Check.ID;
                          dataRow["ФИО"] = Check.UserName;
                          dataRow["Дата"] = Check.Date;
                          dataRow["Программа"] = Check.Program.Name;
                          newTable.Rows.Add(dataRow);
                      }
                      ReportTable.Dispose();
                      ReportTable = newTable;

                      LogText += $"Выполнено. Число выведенных строк - {ParamChecks.Count()}" + Environment.NewLine;

                      ReportFilters.Clear();
                  }));
            }
        }
        #endregion
    }
}
