<template>
  <div>
    <h1>List: {{ list.name }}</h1>
    <hr />
    <table>
      <thead>
        <tr>
          <th>&nbsp;</th>
          <th>Task</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="i in list.items" :key="i.id">
          <td>
            <input
              type="checkbox"
              v-model="i.isCompleted"
              @change="toggleToDoItem(i.id)"
            />
          </td>
          <td>{{ i.text }}</td>
        </tr>
        <tr>
          <td>&nbsp;</td>
          <td>
            <input type="text" v-model.trim="newItemText" />
            <button @click="addNewItem">+</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Vue } from "vue-property-decorator";

@Component
export default class List extends Vue {
  listId = -1;
  newItemText = "";
  list: any = {
    name: "",
    items: [],
  };

  addNewItem() {
    if (this.newItemText == "") return;

    Vue.$connectionService.addToDoItem(this.listId, this.newItemText);
    this.newItemText = "";
  }

  toggleToDoItem(itemId: number) {
    Vue.$connectionService.toggleToDoItem(this.listId, itemId);
  }

  created() {
    this.listId = parseInt(this.$route.params.listId);

    Vue.$connectionService.events.on("updatedListData", (data: any) => {
      this.list = data;
    });

    Vue.$connectionService.getListData(this.listId);
    Vue.$connectionService.subscribeToListUpdates(this.listId);
  }

  destroyed() {
    Vue.$connectionService.unsubscribeFromListUpdates(this.listId);
  }
}
</script>


<style>
</style>