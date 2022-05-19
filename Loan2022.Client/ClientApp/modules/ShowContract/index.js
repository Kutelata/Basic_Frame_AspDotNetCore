import {get} from "@front-end/utils"
//
// export default {
//     selector: '.start-loan .button',
//     handler: async () => {
//         var btn = document.querySelector(".start-loan .button");
//         btn.addEventListener("click", function (e){
//             e.preventDefault();
//             get("/api/check-loan").then((res)=>{
//                 debugger
//             })
//         })
//     },
// };

import loadable from '@loadable/component'

const Widget = loadable(() => import('./Widget'));

export default {
    lazyLoad: false,
    component: Widget
};
