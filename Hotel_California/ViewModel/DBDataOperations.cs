using System;
using System.Collections.Generic;
using System.Linq;
using DAL;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_California.ViewModel
{
    public class DBDataOperations
    {
        private HotelCaliforniaModel db;

        public DBDataOperations()
        {
            db = new HotelCaliforniaModel();
        }

        public reservation GetLastReservation(int room)    //получаем последнюю бронь комнаты
        {
            reservation res = new reservation();
            List<reservation> all = db.reservation.Where(r => r.room == room).ToList();
            if (all.Count == 0)
                return null;
            DateTime maxCheckOut = all.First().check_out_date;
            int reservationId = all.First().id;
            foreach(var item in all)
            {
                if (item.check_out_date >= maxCheckOut)
                {
                    maxCheckOut = item.check_out_date;
                    reservationId = item.id;
                }
            }

            return db.reservation.Where(r => r.id == reservationId).FirstOrDefault();
        }

        public List<room> GetAllRooms()
        {
            return db.room.ToList();
        }

        public List<service> GetServices()
        {
            return db.service.ToList();
        }

        public List<string> GetAllServices()
        {
            List<string> names = new List<string>();
            List<service> list = GetServices();
                foreach (service item in list)
                {
                    names.Add(item.name);
                }

                return names;
        }

        public service GetService(int number)
        {
            return db.service.Where(serv => serv.id == (number + 1)).FirstOrDefault();
        }

        public service GetService(string name)
        {
            return db.service.Where(serv => serv.name == name).FirstOrDefault();
        }

        public List<room> GetRooms(int minimumPrice, int maximumPrice, string type, int capacity, DateTime checkin, DateTime checkout)
        {
            var list = GetRoomList(minimumPrice, maximumPrice, type, capacity);

            reservation last;
            List<room> result = new List<room>();

            foreach (room room in list)
            {
                last = GetLastReservation(room.id);
                if (!(last != null && ((checkin < last.check_in_date && checkout > last.check_in_date) || (checkin <
                    last.check_out_date && checkout > last.check_out_date))))
                {
                    result.Add(room);
                }

            }
            return result;
        }

        public List<room> GetRoomList (int minimumPrice, int maximumPrice, string type, int capacity)
        {
            if (type == "Все")
            {
                return db.room.Where(r => r.price >= minimumPrice && r.price <= maximumPrice && r.capacity >= capacity).ToList();
            }
            else
            {
                return db.room.Where(r => r.price >= minimumPrice && r.roomType.category == type && r.price <=
                maximumPrice && r.capacity >= capacity).ToList();
            }
        }

        public void UpdateReservation(reservation res)
        {
            reservation r = db.reservation.Find(res.id);
            r.total_price = res.total_price;
            r.paid = res.paid;
            Save();
        }
    
        public client GetClient(client client)
        {
            client cl = db.client.Where(c => c.full_name == client.full_name && c.birthdate == client.birthdate &&
            c.phone_number == client.phone_number && c.client_document == client.client_document).FirstOrDefault();

            if (cl != null)
                return cl;
            else return null;
        }

        public reservation GetReservation(int id)
        {
            reservation r = db.reservation.Where(re => re.id == id).FirstOrDefault();

            if (r != null)
                return r;
            else return null;
        }

        public reservation GetReservation(DateTime inday, DateTime outday, int roomnum)
        {
            reservation r = db.reservation.Where(re => re.room1.room_number == roomnum && re.check_in_date == inday 
            && re.check_out_date == outday).FirstOrDefault();

            if (r != null)
                return r;
            else return null;
        }

        public status GetStatus(int id)
        {
            return db.status.Where(s => s.id == id).FirstOrDefault();
        }

        public reservation GetReservation(string name, DateTime bday, DateTime inday, DateTime outday)
        {
            reservation res = db.reservation.Where(r => r.client1.full_name == name && r.client1.birthdate == bday &&
            r.check_in_date == inday && r.check_out_date == outday).FirstOrDefault();

            if (res != null)
                return res;
            else return null;
        }

        public room GetRoom(int number)
        {
            room r = db.room.Where(ro => ro.room_number == number).FirstOrDefault();

            if (r != null)
                return r;
            else return null;
        }

        public room GetRoomId(int number)
        {
            room r = db.room.Where(ro => ro.id == number).FirstOrDefault();

            if (r != null)
                return r;
            else return null;
        }

        public service_string MakeServiceString(reservation res, service service, int amount)
        {
            service_string serv = new service_string();
            serv.service = service.id;
            serv.reservation = res.id;
            serv.cost = amount * service.price;
            serv.service1 = service;
            serv.reservation1 = res;

            return serv;
        }

        public void CreateServiceString(service_string s)
        {
            db.service_string.Add(s);
            Save();
        }

        public void CreateClient(client cl)
        {
            db.client.Add(cl);
            Save();
        }

        public void CreateReservation(reservation res)
        {
            db.reservation.Add(res);
            db.SaveChanges();
        }

        public List<RevenueReport> Report(DateTime begin, DateTime end)
        {
            List<RevenueReport> pre = db.reservation
                .Join(db.room, res => res.room, r => r.id, (res, r) => new { res, r })
                .Where(i => i.res.check_in_date >= begin && i.res.check_out_date <= end)
                .Select(i => new RevenueReport
                {
                    CheckInDate = i.res.check_in_date,
                    CheckOutDate = i.res.check_out_date,
                    RoomCost = (float)i.r.price,
                    ServiceCost = (float)i.res.total_price,
                    TotalCost = (float)i.res.total_price
                })
                .ToList();

            foreach(RevenueReport item in pre)
            {
                item.RoomCost = item.RoomCost * ((item.CheckOutDate.Subtract(item.CheckInDate)).Days);
                item.ServiceCost = item.TotalCost - item.RoomCost;
                item.CheckInDate1 = item.CheckInDate.ToShortDateString();
                item.CheckOutDate1 = item.CheckOutDate.ToShortDateString();
            }
            return pre;

        }

        public bool Save()
        {
            if (db.SaveChanges() > 0) return true;
            return false;
        }
    }
}
