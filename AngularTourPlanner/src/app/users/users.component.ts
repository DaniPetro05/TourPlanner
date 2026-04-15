import { Component, inject, signal } from '@angular/core';
import { User, UserService }  from '../user.service';
import { HttpClient } from '@angular/common/http';
import { ToolbarComponent } from '../toolbar/toolbar.component';

@Component({
  selector: 'app-users',
  imports: [ToolbarComponent],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
  Users = signal<User>({id: 0, username: "", password: "", email: ""});
  users: User = {username: "", password: "", email: ""};
  UserService = inject(UserService);
  requests: any;
  //constructor(private cdr: ChangeDetectorRef) {}

  /*ngOnInit(): void {
    this.getPosts();
  }*/

  getPosts() {
    this.requests = this.UserService.get().subscribe({
      next: (data) => {
        this.users = data;
        console.log(this.users);
        //this.cdr.markForCheck();
      }
    });
  }
}
