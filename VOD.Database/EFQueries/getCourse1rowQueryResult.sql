SELECT TOP(1) [uc].[UserId], [uc].[CourseId], [uc.Course].[Id], [uc.Course].[Description], [uc.Course].[ImageUrl], [uc.Course].[InstructorId], [uc.Course].[MarqueeImageUrl], [uc.Course].[Title], [uc.Course.Instructor].[Id], [uc.Course.Instructor].[Description], [uc.Course.Instructor].[Name], [uc.Course.Instructor].[Thumbnail]
      FROM [UserCourses] AS [uc]
      INNER JOIN [Courses] AS [uc.Course] ON [uc].[CourseId] = [uc.Course].[Id]
      INNER JOIN [Instructors] AS [uc.Course.Instructor] ON [uc.Course].[InstructorId] = [uc.Course.Instructor].[Id]
      WHERE ([uc].[UserId] = '25c705c0-76c8-4296-89d2-2128deb96280') AND ([uc].[CourseId] = 1)