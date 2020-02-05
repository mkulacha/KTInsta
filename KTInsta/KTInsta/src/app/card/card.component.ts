import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.less']
})
export class CardComponent implements OnInit {
  @Input() cardHeadline: string;
  @Input() cardText: string;
  @Input() cardType: number;

  constructor() { }

  ngOnInit() {
  }

}
