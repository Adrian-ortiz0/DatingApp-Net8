import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  standalone: true,
  styleUrls: ['./app.component.css'],
  imports: [NavComponent] 
})
export class AppComponent implements OnInit {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  title = 'DatingApp';
  users: any;

  ngOnInit(): void {
    this.getUsers();
    this.setCurrentUser();
  }
  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString){
      return;
    }
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  getUsers(){
    this.http.get("https://localhost:5001/api/users").subscribe({
      next: response => this.users = response,
      error: error => console.error("Error fetching users", error),
      complete: () => console.log("Request has completed!")
    });
  }
}