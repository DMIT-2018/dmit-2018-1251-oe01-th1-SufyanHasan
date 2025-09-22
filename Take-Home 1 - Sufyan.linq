<Query Kind="Statements">
  <Connection>
    <ID>49534232-1b13-4f32-ab73-97574ec639fa</ID>
    <NamingServiceVersion>2</NamingServiceVersion>
    <Persist>true</Persist>
    <Server>SZAZMA</Server>
    <AllowDateOnlyTimeOnly>true</AllowDateOnlyTimeOnly>
    <Database>StartTed-2025-Sept</Database>
    <DriverData>
      <LegacyMFA>false</LegacyMFA>
    </DriverData>
  </Connection>
</Query>

// Exercise Simple Linq

// Question 1
ClubActivities
    .Where(a => a.StartDate >= new DateTime(2025, 1, 1)
        && a.Name != "BTech Club Meeting"
        && a.CampusVenue.Location != "Scheduled Room")
    .OrderBy(a => a.StartDate)
    .Select(a => new {
        a.StartDate,
        Location = a.CampusVenue.Location,
        Club = a.Club.ClubName,
        Activity = a.Name
    })
    .Dump();
	
// Question 2
Programs
    .Select(p => new {
        School = p.SchoolCode == "SAMIT" ? "School of Advanced Media and IT" :
                 p.SchoolCode == "SEET" ? "School of Electrical Engineering Technology" :
                 "Unknown",
        Program = p.ProgramName,
        RequiredCourseCount = p.ProgramCourses
            .Count(pc => pc.SectionLimit > 0),
        OptionalCourseCount = p.ProgramCourses
            .Count(pc => pc.SectionLimit == 0)
    })
    .Where(x => x.RequiredCourseCount >= 22)
    .OrderBy(x => x.Program)
    .Dump();
	
// Question 3
Students
    .Where(s => s.CountryCode != "CA" 
        && !StudentPayments.Any(p => p.StudentNumber == s.StudentNumber))
    .OrderBy(s => s.LastName)
    .ThenBy(s => s.FirstName)
    .Select(s => new {
        StudentNumber = s.StudentNumber,
        CountryName = Countries
            .Where(c => c.CountryCode == s.CountryCode)
            .Select(c => c.CountryName)
            .FirstOrDefault(),
        FullName = s.FirstName + " " + s.LastName,
        ClubMembershipCount = ClubMembers
            .Count(cm => cm.StudentNumber == s.StudentNumber) == 0
            ? "None"
            : ClubMembers
                .Count(cm => cm.StudentNumber == s.StudentNumber).ToString()
    })
    .Dump();