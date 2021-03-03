﻿using Business.Abstract;
using Core.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;

namespace Business.Concrete
{
    public class CarManager : ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        public IDataResult<List<Car>> GetAll()
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(), Messages.Listed);
        }

        public IDataResult<Car> GetById(int carId)
        {
            return new SuccessDataResult<Car>(_carDal.Get(c=> c.CarID == carId), Messages.Listed);
        }

        public IDataResult<List<Car>> GetCarsByColorId(int colorId)
        {
            return new SuccessDataResult<List<Car>>(_carDal.GetAll(c=> c.ColorID == colorId), Messages.Listed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            return  new SuccessDataResult<List<CarDetailDto>>(_carDal.GetCarDetails(), Messages.Listed);
        }

        [ValidationAspect(typeof(CarValidator), Priority =1)]
        public IResult  Add(Car car)
        {
            if (car.DailyPrice < 0)
            {
                return new ErrorResult(Messages.InvalidEntry);
            }
            else
            {
                _carDal.Add(car);
                return new SuccessResult(Messages.Added);
            }
        }

        public IResult Delete(Car car)
        {
            var result = _carDal.DeleteCarIfNotReturnDateNull(car);
            if (result)
            {
                return new SuccessResult(Messages.Deleted);
            }

            return new ErrorResult(Messages.NotDeleted);
        }

        [ValidationAspect(typeof(CarValidator), Priority =1)]
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.Updated);
        }

       
    }
}
