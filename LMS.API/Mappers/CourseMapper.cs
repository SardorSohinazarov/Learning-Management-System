using System.Linq;
using LMS.API.Models;
using LMS.API.Models.DTO;
using Mapster;

namespace LMS.API.Mappers
{
    public static class CourseMapper
    {
        public static CourseDTO ToDto(this Course course) =>
            new CourseDTO
            {
                Id = course.Id,
                Name = course.Name,
                Key = course.Key,
                Users = course.Users?.Select(userCourse => userCourse.User?.Adapt<UserDTO>()).ToList()
            };
    }
}
