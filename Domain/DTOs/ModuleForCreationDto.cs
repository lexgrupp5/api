﻿namespace Domain.DTOs;

public record ModuleForCreationDto
{
    public string Name { get; set; }
    public int CourseId { get; set; }
    public string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}