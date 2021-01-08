﻿using System;
using Application.Domain.Extensions;
using Application.Domain.Models;
using Application.Domain.Repositories;
using Application.Domain.Repositories.Impl;
using BenchmarkDotNet.Attributes;

namespace Application.Benchmark
{
    public class PersonSqLiteRepositoryBenchmark
    {
        private Bogus.Faker<Person> _personFaker;
        private IPersonRepository _repository;

        [Benchmark]
        public void Create()
        {
            var person = _personFaker.Generate();
            _repository.Post(person);
        }

        [Benchmark]
        public void Delete()
        {
            var person = _repository.Get().RandomElement();
            _repository.Delete(person);
        }

        [Benchmark]
        public void Edit()
        {
            var person = _repository.Get().RandomElement();
            _personFaker.Populate(person);
            _repository.Put(person);
        }

        [Benchmark]
        public void GetAll()
        {
            _repository.Get();
        }

        [GlobalSetup]
        public void Setup()
        {
            _repository = new PersonSqLiteRepository();
            _personFaker = new Bogus.Faker<Person>()
                .RuleFor(p => p.BirthDate, b => b.Date.Past(18))
                .RuleFor(p => p.Height, h => h.Random.Decimal(1.45M, 1.99M))
                .RuleFor(p => p.Name, n => n.Name.FullName())
                .FinishWith((f, p) =>
                {
                    Console.WriteLine("Person Created! Name={0}", p.Name);
                });
        }
    }
}