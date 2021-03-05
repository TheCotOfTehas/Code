
using System;
using System.Dynamic;

namespace Outsourcing
{
    [Route("/apps/cars-api/v1/cars")]
    public class Program : Controller
    {
        private CarsRepository repository;

        [Route("Create/{id}")]
        [HttpPatch]
        public Car Create(MutableRequestPost requestWhichCreate)
        {
            if (requestWhichCreate.Weight > 1000.0 || requestWhichCreate.Weight < 0)
            {
                throw new Exception("Wrong format of weight parameter");
            }

            var car = new Car
            {
                Id = "1",
                Mark = requestWhichCreate.Mark,
                Model = requestWhichCreate.Model,
                Name = requestWhichCreate.Name,
                Weight = requestWhichCreate.Weight
            };
            repository.SaveCar(car);
            return car;
        }

        [Route("UpdateAllCar/{id}")]
        [HttpPost]
        public Car Update(string id, SomeStrangeRequest megaRequest)
        {
            if (megaRequest.Weight > 1000.0 || megaRequest.Weight < 0)
            {
                throw new Exception("Wrong format of weight parameter");
            }

            var car = repository.GetCar(id);
            if (car == null) throw new Exception("Car was not found");

            var updatedCar = repository.UpdateCar(car.Id, megaRequest.Name, megaRequest.Weight);
            return updatedCar;
        }

        [Route("GetCarBy/{id}/inCars")]
        [HttpGet]
        public Car Get(string id)
        {
            var CaaaarModel = repository.GetCar(id);
            if (CaaaarModel == null) throw new Exception("Car was not found");

            return CaaaarModel;
        }

        [Route("DeleteCarsById/{id}")]
        [HttpDelete]
        public Car Delete(string iddddd)
        {
            var car = repository.GetCar(iddddd);
            if (car == null) throw new Exception("Car was not found");

            return car;
        }
    }

    public interface CarsRepository
    {
        Car SaveCar(Car model);

        Car UpdateCar(string id, string Name, double weight);

        Car GetCar(string id);

        void DeleteCar(string id);
    }

    public class Car
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
    }

    public class MutableRequestPost
    {
        public string Name { get; set; }
        public double Weight { get; set; }
        public string Mark { get; set; }
        public string Model { get; set; }
    }

    public class SomeStrangeRequest
    {
        public string Name { get; set; }
        public double Weight { get; set; }
    }
}