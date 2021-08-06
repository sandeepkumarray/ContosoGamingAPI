# Contoso Gaming API
## _Engine for Route/Path search_

### Initialization :
- The data store should contain Landmarks and the distance relation between 2 Landmarks

### Features :
- The distance along certain routes via landmarks
- The number of different routes between landmarks.

### Technology :

- [.Net Core Web API] - Rest API backend.
- [XUnit] - Unit Testing framework.
- [AWS Elastic Beanstalk] - For deploying application to AWS.

Added source code to a public repository on GitHub.

### Swagger UI Help : 

```sh
Step 1:
Call AddRoutes API with following parameters
AB3,BC9,CD3,DE6,AD4,DA5,CE2,AE4,EB1
```
![alt](https://github.com/sandeepkumarray/ContosoGamingAPI/blob/master/Images/AddRoutes.png)

```sh
Step 2:
Call GetDistanceViaRoute API with following parameters
A-B-C
```
![GetDistanceViaRoute](https://raw.githubusercontent.com/sandeepkumarray/ContosoGamingAPI/master/Images/GetDistanceViaRoute.png)

```sh
Step 2:
Call GetRoutes API with following parameters
A/C/2
```
![GetRoutes](https://raw.githubusercontent.com/sandeepkumarray/ContosoGamingAPI/master/Images/GetRoutes.png)

### Development
Dijkstra's algorithm fits best to solve this.

