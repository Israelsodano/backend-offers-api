namespace Offers.Domain.Constants
{
    public class ErrorMessages
    {
        public const string UNIVERSITY_ALREADY_EXISTS = "the university with name: '{0}' already registered";
        public const string UNIVERSITY_NOT_EXISTS = "there is no university registered with this id: '{0}'";

        public const string CAMPUS_ALREADY_EXISTS = "the campus with name: '{0}' already registered";
        public const string CAMPUS_NOT_EXISTS = "there is no campus registered with this id: '{0}'";

        public const string COURSE_ALREADY_EXISTS = "the course with name: '{0}' already registered";
        public const string COURSE_NOT_EXISTS = "there is no course registered with this id: '{0}'";

        public const string OFFER_ALREADY_EXISTS = "the offer with this discount: '{0}' already registered to courseId: '{0}'";
        public const string OFFER_NOT_EXISTS = "there is no offer registered with this id: '{0}'";
    }
}
