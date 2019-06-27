 SELECT [uc].[UserId], [uc].[CourseId], [uc.User].[Id], [uc.User].[AccessFailedCount], [uc.User].[ConcurrencyStamp],
  [uc.User].[Email], [uc.User].[EmailConfirmed], [uc.User].[LockoutEnabled],
   [uc.User].[LockoutEnd], [uc.User].[NormalizedEmail], [uc.User].[NormalizedUserName],
    [uc.User].[PasswordHash], [uc.User].[PhoneNumber], [uc.User].[PhoneNumberConfirmed], 
	[uc.User].[SecurityStamp], [uc.User].[Token], [uc.User].[TokenExpires],
	 [uc.User].[TwoFactorEnabled], [uc.User].[UserName], [uc.Course].[Id], 
	 [uc.Course].[Description], [uc.Course].[ImageUrl], [uc.Course].[InstructorId],
	  [uc.Course].[MarqueeImageUrl], [uc.Course].[Title]
      FROM [UserCourses] AS [uc]
      INNER JOIN [AspNetUsers] AS [uc.User] ON [uc].[UserId] = [uc.User].[Id]
      INNER JOIN [Courses] AS [uc.Course] ON [uc].[CourseId] = [uc.Course].[Id]
      WHERE [uc].[UserId] = '25c705c0-76c8-4296-89d2-2128deb96280'

  SELECT [uc.Course].[Id], [uc.Course].[Description], [uc.Course].[ImageUrl], 
  [uc.Course].[InstructorId], [uc.Course].[MarqueeImageUrl], [uc.Course].[Title]
      FROM [UserCourses] AS [uc]
      INNER JOIN [Courses] AS [uc.Course] ON [uc].[CourseId] = [uc.Course].[Id]
      WHERE [uc].[UserId] = '25c705c0-76c8-4296-89d2-2128deb96280'