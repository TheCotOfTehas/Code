
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Outsourcing
{
    [Route("/apps/cars-api/v1/cars")]
    public class Program : Controller
    {
        public RepositoryCar repositoryCar;

        //POST создает новый ресурс по указанному URI.
        //В теле сообщения запроса содержатся сведения о новом ресурсе.
        //Обратите внимание, что POST также может использоваться для запуска операций,
        //которые фактически не создают ресурсы.
        [Route("Create/{id}")]
        [HttpPost]
        public Car Create(ExtendedRequestMessage requestWhichCreate)
        {
            if (requestWhichCreate.Weight > 1000.0 || requestWhichCreate.Weight < 0)
            {
                throw new Exception("Wrong format of weight parameter");
            }

            var car = new Car
            {
                Id = requestWhichCreate.Id,
                Mark = requestWhichCreate.Mark,
                Model = requestWhichCreate.Model,
                Name = requestWhichCreate.Name,
                Weight = requestWhichCreate.Weight
            };
            repositoryCar.Save(car);
            return car;
        }
        //PATCH выполняет частичное обновление ресурса.
        //В теле запроса указывается набор изменений, применяемых к ресурсу.
        [Route("UpdateAllCar/{id}")]
        [HttpPatch]
        public Car Update(string id, ExtendedRequestMessage megaRequest)
        {
            if (megaRequest.Weight > 1000.0 || megaRequest.Weight < 0)
            {
                throw new Exception("Wrong format of weight parameter");
            }

            var car = repositoryCar.Get(id);
            if (car == null) throw new Exception("Car was not found");

            var updatedCar = repositoryCar.Update(car.Id, megaRequest);
            return updatedCar;
        }
        //GET извлекает представление ресурса по указанному URI.
        //Тело ответного сообщения содержит подробную информацию о запрошенном ресурсе.
        [Route("GetCarBy/{id}/inCars")]
        [HttpGet]
        public Car Get(string id)
        {
            var CaaaarModel = repositoryCar.Get(id);
            if (CaaaarModel == null) throw new Exception("Car was not found");

            return CaaaarModel;
        }

        //DELETE удаляет ресурс по указанному URI.
        [Route("DeleteCarsById/{id}")]
        [HttpDelete]
        public Car Delete(string id)
        {
            var car = repositoryCar.Get(id);
            if (car == null) throw new Exception("Car was not found");
            return car;
        }
    }
    public class RepositoryCar: Repository
    {
        public RepositoryCar()
        {
            repository = new Dictionary<string, Car>();
        }
        Dictionary<string, Car> repository { get; }
        public Car Save(Car newCar)
        {
            if(repository.ContainsKey(newCar.Id)) throw new Exception("A car with this ID is already in the repository");
            repository.Add(newCar.Id, newCar);
            return newCar;
        }
        public Car Update(string id, ChangeWeightName zapros)
        {
            if(!repository.ContainsKey(id))
                throw new Exception("The car with this ID is not in the repository");
            repository[id].Name = zapros.Name;
            repository[id].Weight = zapros.Weight;
            return repository[id];
        }

        public Car Get(string id)
        {
            if(repository.ContainsKey(id))
                throw new Exception("The car with this ID is not in the repository");
            return repository[id];
        }

        public void Delete(string id)
        {
            if (!repository.ContainsKey(id)) 
                throw new Exception("The car with this ID is not in the repository");
            repository.Remove(id);
        }
    }

    public class Car:Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
    }

    public class ExtendedRequestMessage: ChangeWeightName
    {
        public string Mark { get; set; }
        public string Model { get; set; }
    }

    public class ChangeWeightName
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
    }
    public interface Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
    }
    public interface Repository
    {
        Car Save(Car model);

        Car Update(string id, ChangeWeightName zapros);

        Car Get(string id);

        void Delete(string id);
    }
}