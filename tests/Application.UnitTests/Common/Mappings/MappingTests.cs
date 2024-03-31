using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using AutoMapper;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Application.Common.Models;
using CleanArchitectureDDD.Application.Common.Mappings;
using CleanArchitectureDDD.Application.Languages.Queries;
using CleanArchitectureDDD.Domain.Entities;
using NUnit.Framework;
using System;

namespace CleanArchitectureDDD.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config =>
            config.AddMaps(Assembly.GetAssembly(typeof(IConfigDbContext))));

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    /*[Test]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }*/

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return RuntimeHelpers.GetUninitializedObject(type);
    }
}
