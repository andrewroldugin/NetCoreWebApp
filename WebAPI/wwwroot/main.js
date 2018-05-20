(function() {

window.onload = function() {
    Vue.component('editable-span', {
      template: '\
        <span v-if="!edit">\
          {{value}}\
        </span>\
        <input v-else\
               v-bind:value="value"\
               v-bind:type="type"\
               v-on:input="$emit(\'input\', $event.target.value)" />\
    ',
      props: ['edit', 'value', 'type']
    });
    const server = axios.create();

var vue = new Vue({
  el: '#contacs-app',
  data: {
    edit_id: -1,
    new_record: {
        first_name: '',
        last_name: '',
        phone: ''
    },
    edit_restore: {
        first_name: '',
        last_name: '',
        phone: ''
    },
    errors: [],
    items: null,
    loading: false,
    errored: false,
  },
  computed: {
    edit_mode: function() { return this.edit_id != -1; },
  },
  mounted() {
      this.fetchData();
  },
  methods: {
    isPhoneNumber: function(value) {
        // here some regex for phone-validation
        return value.length <= 13;
    },
    checkForm: function(item) {
      this.errors = [];
      if(!item.first_name) { this.errors.push("'First name' required."); }
      if(!item.last_name) { this.errors.push("'Last name' required."); }
      if(!item.phone) { this.errors.push("'Phone' required."); }
      else if(!this.isPhoneNumber(item.phone)) {
        this.errors.push("'Phone' should be valid phone-number.");
      }
      return !this.errors.length;
    },
    editRecordStart: function(index, id) {
        this.edit_id = id;
        this.edit_restore.first_name = this.items[index].first_name;
        this.edit_restore.last_name = this.items[index].last_name;
        this.edit_restore.phone = this.items[index].phone;
    },
    editRecordCancel: function(item) {
        this.edit_id = -1;
        item.first_name = this.edit_restore.first_name;
        item.last_name = this.edit_restore.last_name;
        item.phone = this.edit_restore.phone;
    },
    editRecordDone: function(item) {
        if(!this.checkForm(item)) { return; }
        var that = this;
        that.errors = [];

        server.put('/api/values/' + item.id, item)
        .then(function (response) {
            that.edit_id = -1;
        })
        .catch(function (error) {
            that.errors.push(error.message);
        });
    },
    removeRecord: function(index, id) {
        var that = this;
        that.errors = [];
        server.delete('/api/values/' + id)
        .then(function (response) {
            that.items.splice(index, 1);
        })
        .catch(function (error) {
            that.errors.push(error.message);
        });
    },
    addRecord: function () {
        var that = this;
        that.errors = [];
        if(!this.checkForm(this.new_record)) { return; }
        server.post('/api/values', {
            first_name: this.new_record.first_name,
            last_name: this.new_record.last_name,
            phone: this.new_record.phone
        })
        .then(function (response) {
            that.items.push(response.data);
            that.new_record.first_name = '';
            that.new_record.last_name = '';
            that.new_record.phone = '';
        })
        .catch(function (error) {
            that.errors.push(error.message);
        });
    },
    fetchData: function() {
        var that = this;
        this.loading = true;
        this.errored = false;
        this.errors = [];
        server.get('/api/values', {})
            .then(function (response) {
                that.loading = false;
                that.errored = false;
                that.items = response.data;
            })
            .catch(function (error) {
                that.errored = true;
                that.loading = false;
            });
    }
  }
});

}

})();