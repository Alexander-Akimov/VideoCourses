 SELECT TOP(2) [v].[Id], [v].[CourseId], [v].[Description], [v].[Duration], [v].[ModuleId], [v].[Thumbnail], [v].[Title], [v].[Url],
  [v.Module].[Id], 
  [v.Module].[CourseId], 
  [v.Module].[Title], 

  [v.Course].[Id], 
  [v.Course].[Description], 
  [v.Course].[ImageUrl], 
  [v.Course].[InstructorId], 
  [v.Course].[MarqueeImageUrl], 
  [v.Course].[Title]
      FROM [Videos] AS [v]
      INNER JOIN [Modules] AS [v.Module] ON [v].[ModuleId] = [v.Module].[Id]
      INNER JOIN [Courses] AS [v.Course] ON [v].[CourseId] = [v.Course].[Id]
      WHERE [v].[Id] = 1