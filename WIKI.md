## POZNAJ AI API Documentation

This documentation provides an overview of the REST API for managing courses within our application. The API is built using .NET and offers various endpoints for creating, retrieving, updating, and deleting courses.

### Authentication

All endpoints in this API require authentication via token. Ensure that you include the valid authentication token in the request header to access the protected resources.

### Course Controller

#### Get Course by ID
- **Endpoint:** `GET /api/course/{id}`
- **Description:** Retrieves a course with the specified identifier.
- **Parameters:**
  - `id`: Course identifier.
- **Responses:**
  - `200 OK`: Returns the course with the specified identifier.
  - `404 Not Found`: If the course with the specified identifier is not found.
  - `500 Internal Server Error`: If there is an issue retrieving the course.

#### Create Course
- **Endpoint:** `POST /api/course`
- **Description:** Creates a new course.
- **Request Body:**
  - `courseDto`: Course data to create.
- **Responses:**
  - `201 Created`: If the course was created successfully. Returns the newly added course.
  - `400 Bad Request`: If there are validation errors in the request.
  - `500 Internal Server Error`: If there is an issue creating the course.

#### Update Course
- **Endpoint:** `PUT /api/course/{id}`
- **Description:** Updates a course with the specified identifier.
- **Parameters:**
  - `id`: Course identifier to update.
- **Request Body:**
  - `courseDto`: Course data to update.
- **Responses:**
  - `200 OK`: If the course was updated successfully.
  - `404 Not Found`: If the course with the specified identifier is not found.
  - `500 Internal Server Error`: If there is an issue updating the course.

#### Delete Course
- **Endpoint:** `DELETE /api/course/{id}`
- **Description:** Deletes a course with the specified identifier.
- **Parameters:**
  - `id`: Course identifier to delete.
- **Responses:**
  - `200 OK`: If the course was deleted successfully.
  - `404 Not Found`: If the course with the specified identifier is not found.
  - `500 Internal Server Error`: If there is an issue deleting the course.


### File Upload Controller

The `FileUploadController` manages the uploading and retrieval of video files. It provides endpoints for uploading video files and retrieving specific video files by name.

#### Upload Video
- **Endpoint:** `POST /api/uploads/video`
- **Description:** Uploads a video file and returns its name.
- **Responses:**
  - `200 OK`: Returns if the file was uploaded correctly.
  - `400 Bad Request`: If the file is empty.
  - `500 Internal Server Error`: If there is an issue while uploading the video.

#### Get Video
- **Endpoint:** `GET /api/uploads/video/{fileName}`
- **Description:** Retrieves the specified video file by name.
- **Parameters:**
  - `fileName`: The name of the video file to retrieve.
- **Responses:**
  - `200 OK`: Returns the file.
  - `404 Not Found`: If the file was not found.
  - `500 Internal Server Error`: If there is an issue while retrieving the file.

This controller utilizes Hangfire for asynchronous video conversion and Serilog for logging. Ensure proper authentication and adhere to the request/response structures outlined for each operation.

### Lesson Controller

The `LessonController` handles operations related to lessons, including retrieving, creating, updating, and deleting lessons. Lessons are associated with courses.

#### Get Lesson by ID
- **Endpoint:** `GET /api/lesson/{id}`
- **Description:** Retrieve a lesson with a specified identifier.
- **Parameters:**
  - `id`: Lesson identifier.
- **Responses:**
  - `200 OK`: Returns the lesson with the specified identifier.
  - `404 Not Found`: If the lesson with the specified identifier is not found.
  - `500 Internal Server Error`: If there is an issue while retrieving the lesson.

#### Create Lesson
- **Endpoint:** `POST /api/lesson`
- **Description:** Create a new lesson.
- **Request Body:**
  - `lessonDto`: Lesson data to create.
- **Responses:**
  - `201 Created`: If the lesson was created. Returns the newly added lesson.
  - `404 Not Found`: If the associated course is not found for the lesson.
  - `500 Internal Server Error`: If there is an issue while creating the lesson.

#### Update Lesson
- **Endpoint:** `PUT /api/lesson/{id}`
- **Description:** Update a lesson with a specified identifier.
- **Parameters:**
  - `id`: Lesson identifier to update.
- **Request Body:**
  - `lessonDto`: New lesson data.
- **Responses:**
  - `200 OK`: If the lesson was updated.
  - `404 Not Found`: If the lesson to update is not found.
  - `500 Internal Server Error`: If there is an issue while updating the lesson.

#### Delete Lesson
- **Endpoint:** `DELETE /api/lesson/{id}`
- **Description:** Delete a lesson with a specified identifier.
- **Parameters:**
  - `id`: Lesson identifier to delete.
- **Responses:**
  - `200 OK`: If the lesson was deleted.
  - `404 Not Found`: If the lesson to delete is not found.
  - `500 Internal Server Error`: If there is an issue while deleting the lesson.

### User Controller

The `UserController` manages user-related operations such as login, registration, user retrieval, and course management.

#### Login
- **Endpoint:** `POST /api/user/login`
- **Description:** Logs a user into the system and returns a JWT token after successful login.
- **Request Body:**
  - `model`: Login information.
- **Responses:**
  - `200 OK`: Returns the newly generated JWT token.
  - `400 Bad Request`: If the login information is incorrect.
  - `500 Internal Server Error`: If there is an error during login.

#### Register
- **Endpoint:** `POST /api/user/register`
- **Description:** Registers a new user in the system and returns a JWT token after successful registration.
- **Request Body:**
  - `model`: User information for registration.
- **Responses:**
  - `200 OK`: Returns the newly generated JWT token.
  - `400 Bad Request`: If the user information is invalid or the email is already taken.
  - `500 Internal Server Error`: If there is an error during registration.

#### Get All Users
- **Endpoint:** `GET /api/user`
- **Description:** Retrieves all users.
- **Responses:**
  - `200 OK`: Returns the list of users.
  - `500 Internal Server Error`: If there is an issue while retrieving users.

#### Get User by ID
- **Endpoint:** `GET /api/user/{id}`
- **Description:** Retrieves a user by ID.
- **Parameters:**
  - `id`: User identifier.
- **Responses:**
  - `200 OK`: Returns the user with the specified identifier.
  - `404 Not Found`: If the user with the specified identifier is not found.
  - `500 Internal Server Error`: If there is an issue while retrieving the user.

#### Get All Courses for User
- **Endpoint:** `GET /api/user/courses`
- **Description:** Retrieves all courses for the authenticated user.
- **Responses:**
  - `200 OK`: Returns the list of courses for the user.
  - `401 Unauthorized`: If the user is unauthorized.
  - `500 Internal Server Error`: If there is an issue while retrieving user courses.

#### Check Authentication
- **Endpoint:** `GET /api/user/auth`
- **Description:** Checks if the user is authenticated.
- **Responses:**
  - `200 OK`: Returns a message indicating user authentication status along with user details if authenticated.
  - `401 Unauthorized`: If the token is not found or invalid.

#### Add Course to User
- **Endpoint:** `POST /api/user/courses/{courseId}`
- **Description:** Adds a course to the authenticated user.
- **Parameters:**
  - `courseId`: Course identifier to add.
- **Responses:**
  - `200 OK`: Returns a message indicating that the course was added successfully.
  - `400 Bad Request`: If the ModelState is invalid.
  - `401 Unauthorized`: If the user is unauthorized or the token is not found.
  - `500 Internal Server Error`: If there is an issue while adding a course to the user.

#### Add User Role
- **Endpoint:** `POST /api/user/{userId}/roles`
- **Description:** Adds a role to the specified user.
- **Parameters:**
  - `userId`: User identifier to add the role to.
- **Request Body:**
  - `role`: User role to add.
- **Responses:**
  - `200 OK`: Returns a message indicating that the role was added successfully.
  - `404 Not Found`: If the user is not found.
  - `500 Internal Server Error`: If there is an issue while adding a role to the user.


This documentation serves as a guide for interacting with the provided endpoints. Ensure proper authentication and adhere to the request/response structures outlined for each operation.
