<template>
          <v-container class="ma-2">
            <v-row>
              <h2>{{ item.fullname }}({{ item.code }})</h2>
            </v-row>
            <v-row v-if="!item.accountConfirmed">
              <v-alert text type="warning"
                >This user didn't confirmed his account!</v-alert
              >
            </v-row>
            <v-container class="mt-2">
              <p><strong>Email: </strong>{{ item.email }}</p>
              <p><strong>Role: </strong>{{ item.role }}</p>
              <p><strong>Department: </strong>{{ item.departmentName }}</p>
              <p><strong>Enrollment year: </strong>{{ item.enrollmentYear }}</p>
              <p><strong>Specialization:</strong></p>
              <ul v-for="s in item.userSpecializations" :key="s.id">
                {{
                  s.name
                }}
              </ul>
            </v-container>
          </v-container>
  </template>
  <script lang="ts">
  import { UserViewModule } from "@/modules/user";
  import Vue from "vue";
  
  export default Vue.extend({
    props: {
        item: Object as () =>  UserViewModule
    }
  });
  </script>