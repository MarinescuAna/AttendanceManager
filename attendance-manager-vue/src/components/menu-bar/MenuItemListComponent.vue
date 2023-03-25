<template>
  <div v-if="item.route != ''">
    <MenuItemComponent :item="item" />
  </div>
  <div v-else>
    <v-list-group :prepend-icon="item.icon" no-action>
      <template v-slot:activator>
        <v-list-item-content>
          <v-list-item-title>
            {{ item.title }}
          </v-list-item-title>
        </v-list-item-content>
      </template>
      <MenuItemComponent
        v-for="child in item.children"
        :key="child.title"
        :item="child"
      />
    </v-list-group>
  </div>
</template>


<script lang="ts">
import Vue from "vue";
import { MenuItemListModel } from "../menu-bar/ItemList";
import MenuItemComponent from "./MenuItemComponent.vue";

export default Vue.extend({
  name: "MenuItemListComponent",
  components: {
    MenuItemComponent,
  },
  props: {
    /** Root details */ 
    item:{
      type: Object as () => MenuItemListModel,
    }
  },
});
</script>