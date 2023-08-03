import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root' //root module ---> app module
})
export class AccountService { //is responsible to make our http request from client to server
  //a service lives for the entire lifetime" " of our application, a service can be injected
  //if you watn t set the type of a property you use baseruel:.. ,but if we want to set a value  ""=""
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable(); //$ -->that tells us is an observable



  //inject http client into our constractor
  constructor(private http: HttpClient) { }
  


  //.pipe... to tell the broswer to keep at storage the current user.in order to persist the login after an event.
  login(model: any)//method we eventually call from ur component, argument mode' of type any'
  {
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(  //account is the name of our controller "<>" -> we inform the response is a type User"
      map((response: User) => {
        const user = response;
        if (user) { //check if we have user
          localStorage.setItem('user', JSON.stringify(user))//strinfy object -> to change the value to string,storage needs only string
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model: any)
  {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user))//strinfy object -> to change the value to string,storage needs only string
          this.currentUserSource.next(user)
        }

      })
    )

  }
  
  setCurrentUser(user: User){
    this.currentUserSource.next(user);
  }

  logout(){
    localStorage.removeItem('user'); //remove from storage
    this.currentUserSource.next(null);
  }
}
