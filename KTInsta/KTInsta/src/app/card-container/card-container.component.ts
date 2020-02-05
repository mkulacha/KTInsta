import { Component, OnInit, Inject } from '@angular/core';
import { NgxMasonryOptions } from 'ngx-masonry';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-card-container',
  templateUrl: './card-container.component.html',
  styleUrls: ['./card-container.component.less']
})
export class CardContainerComponent implements OnInit {
  public masonryOptions: NgxMasonryOptions = {
    transitionDuration: '0.2s',
    gutter: 20,
    resize: true,
    initLayout: true,
    fitWidth: true
  };

  public cards = [];
  allCards = [];
  initialCount = 15;
  limit = 5;

  constructor(http: HttpClient) {
    http.get<Card[]>("http://localhost:5000/" + 'api/KTInstaController').subscribe(result => {
      this.allCards = [];
      Object.keys(result).map((key) => { this.allCards.push(result[key]) });
      this.cards = this.allCards.slice(0, this.initialCount);
    }, error => console.error(error));
  }

  async showMoreImages() {
    await this.sleep(10000);
    this.limit += 15;
    this.cards = this.allCards.slice(0, this.limit);
  }

  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  ngOnInit() {
    this.showMoreImages();
  }
}

interface Card {
  cardId: number;
  cardHeadLine: string;
  cardText: string;
  cardType: string;
  dateCreated: string;
}
