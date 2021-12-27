using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Text;
using Hotel_California.View;
using Hotel_California.ViewModel;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using DAL;
using System.IO;
using System.Windows.Controls;

namespace Hotel_California.ViewModel
{
    public class ApplicationViewModel
    {
        protected MainWindow MainWindow;
        protected DBDataOperations dbo;       

        public ApplicationViewModel(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            dbo = new DBDataOperations();
        }

        private BookingWindow booking;
        private RelayCommand bookingCommand;
        public RelayCommand BookingCommand      //открыть страницу поиска и вывести список комнат
        {
            get
            {
                return bookingCommand ??
                  (bookingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          first = false;
                          booking = new BookingWindow(this);

                          MainWindow.stk.Children.Clear();
                          RoomList();        //вывод списка комнат
                          MainWindow.stk.Children.Add(booking);
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }                     
                  }));
            }
        }
        
        private RelayCommand searchingCommand;
        public RelayCommand SearchingCommand    //фильтрованный поиск по комнатам
        {
            get
            {
                return searchingCommand ??
                  (searchingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          first = true;
                          RoomList();     //вывод списка комнат
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        int roomnum;   //номер комнаты
        int type;      //тип (категория) комнаты
        private ReservationWindow reserve;
        private RelayCommand createBookingCommand;    //начало создания бронирования
        public RelayCommand CreateBookingCommand
        {
            get
            {
                return createBookingCommand ??
                  (createBookingCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          reserve = new ReservationWindow(this);

                          MainWindow.stk.Children.Clear();
                          
                          type = ((obj as Button).DataContext as room).room_type;
                          
                          switch (type)
                          {
                              case 1:
                                  reserve.typeLb.Content = "СТАНДАРТ";
                                  break;
                              case 2:
                                  reserve.typeLb.Content = "ЭКОНОМ";
                                  break;
                              case 3:
                                  reserve.typeLb.Content = "ЛЮКС";
                                  break;
                          }

                          //выводим информацию о комнате, которую собираются бронировать:
                          reserve.numberLb.Content = "№" + ((obj as Button).DataContext as room).room_number;
                          roomnum = ((obj as Button).DataContext as room).room_number;
                          reserve.costLb.Content = "ОБЩАЯ СТОИМОСТЬ: " + ((obj as Button).DataContext as room).price 
                          * (booking.outDate.SelectedDate.Value.Subtract(booking.inDate.SelectedDate.Value).Days) + "₽";
                          reserve.capacityLb.Content = "КОЛИЧЕСТВО МЕСТ: " + ((obj as Button).DataContext as room).capacity;
                          reserve.inLb.Content = booking.inDate.SelectedDate.Value;
                          reserve.outLb.Content = booking.outDate.SelectedDate.Value;

                          MainWindow.stk.Children.Add(reserve);

                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }  

        reservation newReservation;   //новая бронь
        client client;           //клиент
        private ServiceWindow serv;
        private RelayCommand continueReservationCommand;   //заказ услуг при бронировании
        public RelayCommand ContinueReservationCommand
        {
            get
            {
                return continueReservationCommand ??
                  (continueReservationCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          serv = new ServiceWindow(this);
                          reserve.bookstk.Children.Clear();

                          client = new client();
                          client.full_name = reserve.fullNameTxb.Text;
                          client.birthdate = reserve.birthDate.SelectedDate.Value;
                          client.phone_number = reserve.phoneNumberTxb.Text;
                          client.client_document = reserve.passportTxb.Text;

                          if (dbo.GetClient(client) == null)    //если этот клиент уже есть в базе
                          {
                              dbo.CreateClient(client);
                          }

                          newReservation = new reservation();
                          newReservation.client = dbo.GetClient(client).id;
                          newReservation.check_in_date = booking.inDate.SelectedDate.Value;
                          newReservation.check_out_date = booking.outDate.SelectedDate.Value;
                          newReservation.room = dbo.GetRoom(roomnum).id;
                          newReservation.status = 1;
                          newReservation.paid = 0;
                          newReservation.amount_of_guests = (int)booking.peopleAmountUpDown.Value;
                          newReservation.total_price = dbo.GetRoomId(newReservation.room).price * (booking.outDate.SelectedDate.Value.Subtract(booking.inDate.SelectedDate.Value).Days);

                          List<string> services = new List<string>();

                          //заполняем комбобокс с услугаим
                          services = dbo.GetAllServices();
                          serv.serviceComboBox.Items.Clear();
                          serv.serviceComboBox.ItemsSource = services;
                          serv.serviceComboBox.SelectedItem = services.First();
                          
                          dbo.CreateReservation(newReservation);

                          reserve.bookstk.Children.Add(serv);

                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private RelayCommand moreServCommand;
        public RelayCommand MoreServCommand  //добавить ещё одну услугу
        {
            get
            {
                return moreServCommand ??
                  (moreServCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          //сохраняем предыдущую выбранную услугу
                          newReservation.service_string.Add(dbo.MakeServiceString(newReservation, dbo.GetService
                              (serv.serviceComboBox.SelectedIndex), (int)serv.serviceAmountUpDown.Value));
                          newReservation.total_price += newReservation.service_string.Last().cost;
                          serv.serviceAmountUpDown.Value = 1;
                          dbo.CreateServiceString(newReservation.service_string.Last());
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private RelayCommand refuseServCommand;
        public RelayCommand RefuseServCommand    //сбросить выбранную услугу и закончить бронь
        {
            get
            {
                return refuseServCommand ??
                  (refuseServCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          MessageBox.Show("Бронирование успешно создано!");

                          //возвращаемся назад на страницу со списком комнат
                          MainWindow.stk.Children.Clear();
                          RoomList();
                          MainWindow.stk.Children.Add(booking);

                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        protected List<room> RoomsList;       //список комнат
        protected bool first;        //параметр для вывода списка комнат: если false, значит, ещё не было заполнения фильтров и выводятся все комнаты

        public void RoomList()     //выводит комнаты с учётом поставленных фильтров
        {
            int peopleAmount = (int)booking.peopleAmountUpDown.Value;
            int minPrice = (int)booking.minPriceUpDown.Value;
            int maxPrice = (int)booking.maxPriceUpDown.Value;
            DateTime indate = (DateTime)booking.inDate.SelectedDate;
            DateTime outdate = (DateTime)booking.outDate.SelectedDate;
            string RoomType = " ";
            int roomType = (int)booking.roomTypeComboBox.SelectedIndex;

            switch (roomType)
            {
                case 0:
                    RoomType = "Все";
                    break;
                case 1:
                    RoomType = "Стандарт";
                    break;
                case 2:
                    RoomType = "Эконом";
                    break;
                case 3:
                    RoomType = "Люкс";
                    break;
            }

            if (first == false)
            {
                RoomsList = dbo.GetAllRooms();
            }
            else if (first == true)
            {
                RoomsList = dbo.GetRooms(minPrice, maxPrice, RoomType, peopleAmount, indate, outdate);
            }

            booking.listing.Children.Clear();

            int marg = 0;

            foreach (room room in RoomsList)
            {
                Button roomButton = new Button();

                switch (room.roomType.category)
                {
                    case "Стандарт":
                        {
                            roomButton = (Button)booking.TryFindResource("standardButton");
                            switch (room.capacity)
                            {
                                case 1:
                                    roomButton.Content = "СТАНДАРТ  " + room.capacity + " МЕСТО  " + room.price + "₽";
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    roomButton.Content = "СТАНДАРТ  " + room.capacity + " МЕСТА  " + room.price + "₽";
                                    break;
                                case 5:
                                    roomButton.Content = "СТАНДАРТ  " + room.capacity + " МЕСТ  " + room.price + "₽";
                                    break;
                            }
                            break;
                        }
                    case "Эконом":
                        {
                            roomButton = (Button)booking.TryFindResource("economyButton");
                            switch (room.capacity)
                            {
                                case 1:
                                    roomButton.Content = "ЭКОНОМ  " + room.capacity + " МЕСТО " + room.price + "₽";
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    roomButton.Content = "ЭКОНОМ  " + room.capacity + " МЕСТА  " + room.price + "₽";
                                    break;
                                case 5:
                                    roomButton.Content = "ЭКОНОМ  " + room.capacity + " МЕСТ  " + room.price + "₽";
                                    break;
                            }
                            break;
                        }
                    case "Люкс":
                        {
                            roomButton = (Button)booking.TryFindResource("luxButton");
                            switch (room.capacity)
                            {
                                case 1:
                                    roomButton.Content = "ЛЮКС  " + room.capacity + " МЕСТО  " + room.price + "₽";
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    roomButton.Content = "ЛЮКС  " + room.capacity + " МЕСТА  " + room.price + "₽";
                                    break;
                                case 5:
                                    roomButton.Content = "ЛЮКС  " + room.capacity + " МЕСТ  " + room.price + "₽";
                                    break;
                            }
                            break;
                        }
                }

                roomButton.Margin = new Thickness(0, 60 * marg + 10, 0, 20);  //расстояние между кнопками
                marg++;
                booking.listing.Children.Add(roomButton);
            }
        }

        private RelayCommand reservationCompCommand;
        public RelayCommand ReservationCompCommand        //сохранить выбранную услугу и закончить бронь
        {
            get
            {
                return reservationCompCommand ??
                  (reservationCompCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          newReservation.service_string.Add(dbo.MakeServiceString(newReservation, dbo.GetService
                              (serv.serviceComboBox.SelectedIndex), (int)serv.serviceAmountUpDown.Value));
                          newReservation.total_price += newReservation.service_string.Last().cost;                          
                          dbo.CreateServiceString(newReservation.service_string.Last());
                          MessageBox.Show("Бронирование успешно создано!");
                          MainWindow.stk.Children.Clear();
                          RoomList();
                          MainWindow.stk.Children.Add(booking);
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        private RevenueWindow revenueRep;
        private RelayCommand incomeCommand;       //переход на страницу отчёта
        public RelayCommand IncomeCommand
        {
            get
            {
                return incomeCommand ??
                  (incomeCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          revenueRep = new RevenueWindow(this);

                          MainWindow.stk.Children.Clear();
                          MainWindow.stk.Children.Add(revenueRep);
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }

        List<RevenueReport> report;
        private RelayCommand revenueCommand;  
        public RelayCommand RevenueCommand        //сформировать отчёт по выручке за определённый промежуток времени
        {
            get
            {
                return revenueCommand ??
                  (revenueCommand = new RelayCommand(obj =>
                  {
                      try
                      {
                          report = dbo.Report(revenueRep.beginDate.SelectedDate.Value, revenueRep.endDate.SelectedDate.Value);
                          //revenueRep.revenueGrd.ColumnWidth = DataGridLength.SizeToHeader;
                          //revenueRep.revenueGrd.ColumnWidth = DataGridLength.SizeToCells;
                          revenueRep.revenueGrd.ItemsSource = report;
                      }
                      catch (Exception ex)
                      {
                          MessageBox.Show(ex.Message);
                      }
                  }));
            }
        }
    }
}


                