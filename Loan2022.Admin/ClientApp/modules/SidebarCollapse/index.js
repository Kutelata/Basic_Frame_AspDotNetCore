import $ from 'jquery'

export default {
    selector: '.sidebar-mini',
    handler: async () => {
        // console.log($('.pushmenu'))
        document.querySelector('.pushmenu').addEventListener('click', function () {
            var sidebar = document.querySelector('.sidebar-mini');
            sidebar.classList.toggle('sidebar-collapse');
        })
    },
};
