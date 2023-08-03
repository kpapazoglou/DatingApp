import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'Dating app';
 
  
  constructor(private accountService: AccountService) {}
  
  ngOnInit(): void {
    this.setCurrentUser(); //go to setCurrentUser and set a user from locastorage if we got any 
   

  }


  //method to set the current user
  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user: User = JSON.parse(userString); //const user of type User, 
    this.accountService.setCurrentUser(user);
  }


  
}
