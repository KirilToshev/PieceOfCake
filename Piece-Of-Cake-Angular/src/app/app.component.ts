import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public dishes?: Dish[];

  constructor(http: HttpClient) {
    http.get <Envelope<Dish[]>>('/api/dishes').subscribe(responseBody => {
      this.dishes = responseBody.result;
    }, error => console.error(error));
  }

  title = 'Piece-Of-Cake-Angular';
}

interface Envelope<T> {
  result: T,
  errorMessage: string,
  timeGenerated: Date
}

interface Dish {
  id: number;
  name: string;
  description: string;
  state: string;
}
