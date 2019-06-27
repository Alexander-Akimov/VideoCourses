Select v.*
from Videos as v inner join Courses as c on v.CourseId = c.Id inner join UserCourses as uc on uc.CourseId = c.Id
where v.ModuleId = 1 and uc.UserID = '25c705c0-76c8-4296-89d2-2128deb96280'