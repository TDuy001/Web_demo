using AutoMapper;
using Demo3.Data.Entities;

namespace Demo3.ViewModels.AutoMapper
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // course
            CreateMap<Course, CourseViewModel>();
            CreateMap<CourseViewModel, Course>();
            CreateMap<CourseRequest, Course>();

            //lesson
            CreateMap<Lesson, LessonViewModel>();
            CreateMap<LessonViewModel, Lesson>();
            CreateMap<LessonRequest, Lesson>()
            .ForMember(des => des.DateCreated, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}