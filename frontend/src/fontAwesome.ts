import type { App } from "vue";
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

import { library } from "@fortawesome/fontawesome-svg-core";
import {
  faArrowLeft,
  faArrowRight,
  faArrowRightFromBracket,
  faArrowRightToBracket,
  faArrowUpRightFromSquare,
  faAt,
  faBan,
  faCheck,
  faCircleInfo,
  faEnvelope,
  faHome,
  faIdCard,
  faKey,
  faLock,
  faPhone,
  faPlus,
  faRobot,
  faSave,
  faTimes,
  faTrashCan,
  faUser,
  faVial,
} from "@fortawesome/free-solid-svg-icons";

library.add(
  faArrowLeft,
  faArrowRight,
  faArrowRightFromBracket,
  faArrowRightToBracket,
  faArrowUpRightFromSquare,
  faAt,
  faBan,
  faCheck,
  faCircleInfo,
  faEnvelope,
  faHome,
  faIdCard,
  faKey,
  faLock,
  faPhone,
  faPlus,
  faRobot,
  faSave,
  faTimes,
  faTrashCan,
  faUser,
  faVial,
);

export default function (app: App) {
  app.component("font-awesome-icon", FontAwesomeIcon);
}
