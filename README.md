# Education Manual

Simple Web application for online lessons. 

## How it works
Users role in the system: SuperAdmin, SchoolAdmin, Teacher, Student <br />
SuperAdmin creates and edit Schools and Classrooms. Also he can register new SuperAdmin, SchoolAdmin, Teacher, Student. <br />
SchoolAdmin creates and edit Classrooms. Also he can register new SchoolAdmin, Teacher, Student. <br />
Teacher starts new lesson for Students in selected Classroom and select Tasks for each Student. <br />
Student receives Tasks from Teacher and performs it.

## How start

1. Clone this repository
`git clone https://github.com/Vals0ray/EducationManual.git`
2. Restore packages in NuGet 
3. Build project
4. Run project (Code first will create database automatically)
5. Log in as SuperAdmin login and password 111111

## Tasks completed

- [x] Users in roles logic
- [x] Schools logic
- [x] Classrooms logic
- [ ] Tasks for students
- [ ] Recommendation system for teachers

## Screenshots of the Frontend

### Login the system
![alt text](https://i.imgur.com/32NgYkZ.jpg)
### List of the Schools
![alt text](https://i.imgur.com/Lz2ELIQ.jpg)
### List of the Classrooms in selected School
![alt text](https://i.imgur.com/zfVtXxt.jpg)
### List of the Users in Student role
![alt text](https://i.imgur.com/f6Ro4km.jpg)
### Register new Teacher
![alt text](https://i.imgur.com/35JcwEo.jpg)
