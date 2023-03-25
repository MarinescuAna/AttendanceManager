<template>
  <v-container justify="center">
    <v-row v-if="isLogged">
      <v-card
        min-width="30%"
        class="ma-4"
        v-for="link in links"
        :key="link.title"
      >
        <v-card-text>
          <div v-if="link.parentName !== ''">{{ link.parentName }}</div>
          <p class="text-h4 text--primary">{{ link.title }}</p>
          <div class="text--primary">
            {{ link.description }}
          </div>
        </v-card-text>
        <v-card-actions>
          <v-btn
            text
            color="teal accent-4"
            @click="$router.push({ name: link.route })"
          >
            Go to this page
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-row>
  </v-container>
</template>

<style>
.v-card--reveal {
  bottom: 0;
  opacity: 1 !important;
  position: absolute;
  width: 100%;
}
</style>

<script lang="ts">
import { MenuItems } from "@/components/menu-bar/ItemList";
import AuthService from "@/services/auth.service";
import { Role } from "@/shared/enums";
import Vue from "vue";

interface ItemModel {
  parentName: string;
  title: string;
  route: string;
  description: string;
}

export default Vue.extend({
  name: "HomeView",
  data() {
    return {
      links: [] as ItemModel[],
      isLogged: false,
      reveal: false,
    };
  },
  created(): void {
    this.isLogged = AuthService.isLogged();

    if (this.isLogged) {
      const data = AuthService.getDataFromToken();

      if (data !== null) {
        MenuItems.getLinkListByRole(Role[data.role]).forEach((x) => {
          if (x.title !== "Home") {
            if (x.children.length > 0) {
              x.children.forEach((child) => {
                this.links.push({
                  parentName: x.title,
                  description: child.description,
                  route: child.route,
                  title: child.title,
                } as ItemModel);
              });
            } else {
              this.links.push({
                description: x.description,
                route: x.route,
                title: x.title,
                parentName: "",
              } as ItemModel);
            }
          }
        });
      }
    }
  },
});
</script>
