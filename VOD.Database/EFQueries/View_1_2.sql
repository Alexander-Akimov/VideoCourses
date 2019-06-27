SELECT        v.Id, v.Title, v.Description, v.Thumbnail, v.Url, v.Duration, v.ModuleId, v.CourseId
FROM            dbo.Videos AS v INNER JOIN
                         dbo.Courses AS c ON v.CourseId = c.Id INNER JOIN
                         dbo.UserCourses AS uc ON uc.CourseId = c.Id
WHERE        (v.ModuleId = 1) AND (uc.UserId = '25c705c0-76c8-4296-89d2-2128deb96280')

SELECT        video.Id, video.CourseId, video.Description, video.Duration, video.ModuleId, video.Thumbnail, video.Title, video.Url
FROM            dbo.Videos AS video INNER JOIN
                         dbo.Courses AS c ON video.CourseId = c.Id INNER JOIN
                         dbo.UserCourses AS uc ON c.Id = uc.CourseId
WHERE        (video.ModuleId = 1) AND (uc.UserId = '25c705c0-76c8-4296-89d2-2128deb96280')