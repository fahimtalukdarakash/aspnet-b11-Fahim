using XmlFormattingAssignment;
using XmlFormattingAssignmentTest.TestCase1;
using System.Net;
using System.Diagnostics;

namespace XmlFormattingAssignmentTest
{
    public class TestXmlFormatter
    {
        Course _course;

        [SetUp]
        public void Setup()
        {
            _course = new Course();
            _course.Title = "Asp.net";
            _course.Fees = 30000;
            _course.Teacher = new Instructor()
            {
                Name = "Jalaluddin"
            };
            Address presentAddress = new Address();
            presentAddress.Street = "101";
            presentAddress.City = "Dhaka";
            presentAddress.Country = "Bangladesh";

            Address permanentAddress = new Address();
            permanentAddress.Street = "102";
            permanentAddress.City = "Rangpur";
            permanentAddress.Country = "Bangladesh";

            _course.Teacher.PresentAddress = presentAddress;
            _course.Teacher.PermanentAddress = permanentAddress;
            _course.StartDate = new DateTime(2022, 12, 1);

            _course.Tests = new List<AdmissionTest>();
            _course.Tags = new string[] { "C#", "HTML", "CSS" };

            string[] stringArray = new string[] { "Hello", "World" };

            List<Course> CourseList = new List<Course>();
            CourseList.Add(_course);
            CourseList.Add(_course);
            CourseList.Add(_course);
            CourseList.Add(_course);

            AdmissionTest admissionTest1 = new AdmissionTest
            {
                StartDateTime = new DateTime(2022, 10, 3, 9, 9, 9),
                EndDateTime = new DateTime(2022, 10, 3, 11, 11, 11),
                TestFees = 100
            };
            AdmissionTest admissionTest2 = new AdmissionTest
            {
                StartDateTime = new DateTime(2022, 11, 3, 9, 9, 9),
                EndDateTime = new DateTime(2022, 11, 3, 10, 10, 10),
                TestFees = 150
            };

            _course.Tests = new List<AdmissionTest>();
            _course.Tests.Add(admissionTest1);
            _course.Tests.Add(admissionTest2);
        }

        [Test]
        public void Test1()
        {
            var expectedResult = """
                <Course>
                	<Id></Id>
                	<Title>Asp.net</Title>
                	<Supervisor></Supervisor>
                	<Teacher>
                		<Name>Jalaluddin</Name>
                		<Email></Email>
                		<PresentAddress>
                			<Street>101</Street>
                			<City>Dhaka</City>
                			<Country>Bangladesh</Country>
                		</PresentAddress>
                		<PermanentAddress>
                			<Street>102</Street>
                			<City>Rangpur</City>
                			<Country>Bangladesh</Country>
                		</PermanentAddress>
                		<PhoneNumbers></PhoneNumbers>
                	</Teacher>
                	<Topics></Topics>
                	<Fees>30000</Fees>
                	<Tests>
                		<AdmissionTest>
                			<StartDateTime>10/3/2022 9:09:09 AM</StartDateTime>
                			<EndDateTime>10/3/2022 11:11:11 AM</EndDateTime>
                			<TestFees>100</TestFees>
                		</AdmissionTest>
                		<AdmissionTest>
                			<StartDateTime>11/3/2022 9:09:09 AM</StartDateTime>
                			<EndDateTime>11/3/2022 10:10:10 AM</EndDateTime>
                			<TestFees>150</TestFees>
                		</AdmissionTest>
                	</Tests>
                	<Tags>
                		<String>C#</String>
                		<String>HTML</String>
                		<String>CSS</String>
                	</Tags>
                	<StartDate>12/1/2022 12:00:00 AM</StartDate>
                </Course>
                """;
            string result = XmlFormatter.Convert(_course);
            Debug.WriteLine(result);
            Assert.That(RemoveWhitespace(result), Is.EqualTo(RemoveWhitespace(expectedResult)));
        }

        private string RemoveWhitespace(string input)
        {
            return string.Join("", input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }
    }
}