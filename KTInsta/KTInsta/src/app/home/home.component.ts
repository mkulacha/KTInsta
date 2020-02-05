import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less']
})
export class HomeComponent implements OnInit {

  continueCycle: boolean = true;
  currentScroll: number = 0;

  constructor() { }

  ngOnInit() {
    this.infiniteScroll();
  }

  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  async infiniteScroll() {
    var CurrentScroll = 0;
    while (this.continueCycle) {
      await this.sleep(10);
      window.scrollTo({ top: this.currentScroll, behavior: 'smooth' });
      this.currentScroll += 0.25;
    }
  }

}
