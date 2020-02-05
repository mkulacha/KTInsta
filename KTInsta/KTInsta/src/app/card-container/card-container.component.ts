import { Component, OnInit } from '@angular/core';
import { NgxMasonryOptions } from 'ngx-masonry';

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
  masonryImages;
  limit = 15;
  dummyCards = [
    {
      cardHeadline: 'https://source.unsplash.com/433x649/?Uruguay'
    },
    {
      cardHeadline: 'https://source.unsplash.com/530x572/?Jamaica'
    },
    {
      cardHeadline: 'https://source.unsplash.com/531x430/?Kuwait'
    },
    {
      cardHeadline: 'https://source.unsplash.com/586x1073/?Bermuda'
    },
    {
      cardHeadline: 'https://source.unsplash.com/500x571/?Ecuador'
    },
    {
      cardHeadline: 'https://source.unsplash.com/579x518/?Virgin Islands (British)'
    },
    {
      cardHeadline: 'https://source.unsplash.com/503x548/?Angola'
    },
    {
      cardHeadline: 'https://source.unsplash.com/511x630/?Mauritania'
    },
    {
      cardHeadline: 'https://source.unsplash.com/414x767/?Sri Lanka'
    },
    {
      cardHeadline: 'https://source.unsplash.com/443x704/?St. Helena'
    },
    {
      cardHeadline: 'https://source.unsplash.com/441x1145/?Namibia'
    },
    {
      cardHeadline: 'https://source.unsplash.com/491x1097/?Samoa'
    },
    {
      cardHeadline: 'https://source.unsplash.com/570x851/?Eritrea'
    },
    {
      cardHeadline: 'https://source.unsplash.com/560x1072/?Iraq'
    },
    {
      cardHeadline: 'https://source.unsplash.com/551x598/?Togo'
    },
    {
      cardHeadline: 'https://source.unsplash.com/518x813/?Romania'
    },
    {
      cardHeadline: 'https://source.unsplash.com/497x524/?Kenya'
    },
    {
      cardHeadline: 'https://source.unsplash.com/549x826/?Martinique'
    },
    {
      cardHeadline: 'https://source.unsplash.com/559x627/?Tokelau'
    },
    {
      cardHeadline: 'https://source.unsplash.com/594x513/?Belize'
    },
    {
      cardHeadline: 'https://source.unsplash.com/480x1181/?Virgin Islands (US)'
    },
    {
      cardHeadline: 'https://source.unsplash.com/526x552/?Chile'
    },
    {
      cardHeadline: 'https://source.unsplash.com/427x504/?Western Sahara'
    },
    {
      cardHeadline: 'https://source.unsplash.com/468x971/?St. Pierre and Miquelon'
    },
    {
      cardHeadline: 'https://source.unsplash.com/419x790/?Thailand'
    },
    {
      cardHeadline: 'https://source.unsplash.com/417x1125/?Myanmar'
    },
    {
      cardHeadline: 'https://source.unsplash.com/480x632/?Cocos (Keeling Islands)'
    },
    {
      cardHeadline: 'https://source.unsplash.com/416x900/?Belarus'
    },
    {
      cardHeadline: 'https://source.unsplash.com/408x837/?Panama'
    },
    {
      cardHeadline: 'https://source.unsplash.com/541x1021/?Slovak Republic'
    },
    {
      cardHeadline: 'https://source.unsplash.com/597x1107/?Malta'
    },
    {
      cardHeadline: 'https://source.unsplash.com/487x842/?Bahrain'
    },
    {
      cardHeadline: 'https://source.unsplash.com/403x904/?Somalia'
    },
    {
      cardHeadline: 'https://source.unsplash.com/544x515/?Morocco'
    },
    {
      cardHeadline: 'https://source.unsplash.com/577x1044/?Djibouti'
    },
    {
      cardHeadline: 'https://source.unsplash.com/404x437/?France, Metropolitan'
    },
    {
      cardHeadline: 'https://source.unsplash.com/482x1079/?Libya'
    },
    {
      cardHeadline: 'https://source.unsplash.com/473x887/?Bolivia'
    },
    {
      cardHeadline: 'https://source.unsplash.com/583x614/?Kazakhstan'
    },
    {
      cardHeadline: 'https://source.unsplash.com/592x838/?Guyana'
    },
    {
      cardHeadline: 'https://source.unsplash.com/422x731/?Switzerland'
    },
    {
      cardHeadline: 'https://source.unsplash.com/448x985/?Venezuela'
    },
    {
      cardHeadline: 'https://source.unsplash.com/494x936/?Uzbekistan'
    },
    {
      cardHeadline: 'https://source.unsplash.com/461x762/?Benin'
    },
    {
      cardHeadline: 'https://source.unsplash.com/550x676/?Palau'
    },
    {
      cardHeadline: 'https://source.unsplash.com/524x901/?Laos'
    },
    {
      cardHeadline: 'https://source.unsplash.com/594x1199/?French Southern Territories'
    },
    {
      cardHeadline: 'https://source.unsplash.com/557x662/?Malawi'
    },
    {
      cardHeadline: 'https://source.unsplash.com/428x1184/?Swaziland'
    },
    {
      cardHeadline: 'https://source.unsplash.com/433x631/?Lithuania'
    },
    {
      cardHeadline: 'https://source.unsplash.com/523x477/?Sweden'
    },
    {
      cardHeadline: 'https://source.unsplash.com/568x443/?Bahamas'
    },
    {
      cardHeadline: 'https://source.unsplash.com/434x448/?United Arab Emirates'
    },
    {
      cardHeadline: 'https://source.unsplash.com/566x740/?Puerto Rico'
    },
    {
      cardHeadline: 'https://source.unsplash.com/481x580/?Nicaragua'
    },
    {
      cardHeadline: 'https://source.unsplash.com/470x761/?Viet Nam'
    },
    {
      cardHeadline: 'https://source.unsplash.com/519x954/?Australia'
    },
    {
      cardHeadline: 'https://source.unsplash.com/405x1030/?Marshall Islands'
    },
    {
      cardHeadline: 'https://source.unsplash.com/587x1059/?Falkland Islands (Malvinas)'
    },
    {
      cardHeadline: 'https://source.unsplash.com/593x907/?Montserrat'
    },
    {
      cardHeadline: 'https://source.unsplash.com/462x729/?British Indian Ocean Territory'
    },
    {
      cardHeadline: 'https://source.unsplash.com/538x610/?Norway'
    },
    {
      cardHeadline: 'https://source.unsplash.com/506x1057/?Malaysia'
    },
    {
      cardHeadline: 'https://source.unsplash.com/588x756/?Anguilla'
    },
    {
      cardHeadline: 'https://source.unsplash.com/468x1047/?Senegal'
    },
    {
      cardHeadline: 'https://source.unsplash.com/574x498/?Zaire'
    },
    {
      cardHeadline: 'https://source.unsplash.com/594x635/?Ireland'
    },
    {
      cardHeadline: 'https://source.unsplash.com/448x759/?Nauru'
    },
    {
      cardHeadline: 'https://source.unsplash.com/540x619/?Mayotte'
    },
    {
      cardHeadline: 'https://source.unsplash.com/552x438/?Syria'
    },
    {
      cardHeadline: 'https://source.unsplash.com/563x759/?Ghana'
    },
    {
      cardHeadline: 'https://source.unsplash.com/569x1171/?Austria'
    },
    {
      cardHeadline: 'https://source.unsplash.com/581x657/?Macau'
    },
    {
      cardHeadline: 'https://source.unsplash.com/539x1013/?Mozambique'
    },
    {
      cardHeadline: 'https://source.unsplash.com/581x477/?Liechtenstein'
    },
    {
      cardHeadline: 'https://source.unsplash.com/495x589/?Saint Vincent and The Grenadines'
    },
    {
      cardHeadline: 'https://source.unsplash.com/579x680/?Brazil'
    },
    {
      cardHeadline: 'https://source.unsplash.com/467x642/?Turks and Caicos Islands'
    },
    {
      cardHeadline: 'https://source.unsplash.com/496x1186/?Italy'
    },
    {
      cardHeadline: 'https://source.unsplash.com/416x844/?Reunion'
    },
    {
      cardHeadline: 'https://source.unsplash.com/442x569/?Sierra Leone'
    },
    {
      cardHeadline: 'https://source.unsplash.com/517x1084/?Northern Mariana Islands'
    },
    {
      cardHeadline: 'https://source.unsplash.com/431x1164/?Belgium'
    },
    {
      cardHeadline: 'https://source.unsplash.com/541x649/?Netherlands'
    },
    {
      cardHeadline: 'https://source.unsplash.com/491x902/?Korea (South)'
    },
    {
      cardHeadline: 'https://source.unsplash.com/534x778/?Guinea'
    },
    {
      cardHeadline: 'https://source.unsplash.com/528x933/?Tunisia'
    },
    {
      cardHeadline: 'https://source.unsplash.com/441x609/?Tonga'
    },
    {
      cardHeadline: 'https://source.unsplash.com/469x1060/?Equatorial Guinea'
    },
    {
      cardHeadline: 'https://source.unsplash.com/588x794/?S. Georgia and S. Sandwich Isls.'
    },
    {
      cardHeadline: 'https://source.unsplash.com/536x1103/?Algeria'
    },
    {
      cardHeadline: 'https://source.unsplash.com/541x821/?Israel'
    },
    {
      cardHeadline: 'https://source.unsplash.com/414x541/?Bulgaria'
    },
    {
      cardHeadline: 'https://source.unsplash.com/477x879/?Turkmenistan'
    },
    {
      cardHeadline: 'https://source.unsplash.com/450x777/?Croatia (Hrvatska)'
    },
    {
      cardHeadline: 'https://source.unsplash.com/528x978/?Cook Islands'
    },
    {
      cardHeadline: 'https://source.unsplash.com/521x686/?Solomon Islands'
    },
    {
      cardHeadline: 'https://source.unsplash.com/550x432/?Bosnia and Herzegovina'
    },
    {
      cardHeadline: 'https://source.unsplash.com/540x631/?Tanzania'
    },
    {
      cardHeadline: 'https://source.unsplash.com/594x443/?Chad'
    }
  ];

  ngOnInit() {
    this.masonryImages = this.dummyCards.slice(0, this.limit);
  }
}
