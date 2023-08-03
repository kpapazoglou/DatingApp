import { Component, EnvironmentInjector, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}; //a new class names any' , to access this model model.username  , model.password


  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    
  }

 

  login(){
    this.accountService.login(this.model).subscribe({
      next: response => { //
        console.log(response); //to see what we can from our response
      },
      error: error => console.log(error) //put the error on console
      })
      
  }
  logout(){
    this.accountService.logout();
  }

}
