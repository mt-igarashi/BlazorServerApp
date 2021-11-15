var app = new Vue({
    el: '#app',
    data: {
        items: []
    },
    methods: {
        getMovieList(json) {      
            if (json) {
              this.items = JSON.parse(json);
            }
          },
    }
})

export function getMovieList(json)
{
    app.getMovieList(json);
}