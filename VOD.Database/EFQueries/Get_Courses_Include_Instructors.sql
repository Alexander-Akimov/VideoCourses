 SELECT [c].[Id], [c].[Description], [c].[ImageUrl], [c].[InstructorId], [c].[MarqueeImageUrl], [c].[Title], [c.Instructor].[Id], [c.Instructor].[Description], [c.Instructor].[Name], [c.Instructor].[Thumbnail]
      FROM [Courses] AS [c]
      INNER JOIN [Instructors] AS [c.Instructor] ON [c].[InstructorId] = [c.Instructor].[Id]