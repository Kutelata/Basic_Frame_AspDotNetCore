//https://usehooks.com/uselink/
import {useState, useEffect} from 'react';

export const useStylesheet = (src) => {
  // Keep track of link status ("idle", "loading", "ready", "error")
  const [status, setStatus] = useState(src ? "loading" : "idle");
  useEffect(
    () => {
      // Allow falsy src value if waiting on other data needed for
      // constructing the link URL passed to this hook.
      if (!src) {
        setStatus("idle");
        return;
      }
      // Fetch existing link element by src
      // It may have been added by another intance of this hook
      let link = document.querySelector(`link[href="${src}"]`);
      if (!link) {
        // Create link
        link = document.createElement("link");
        link.href = src;
        link.media = 'all';
        link.rel = 'stylesheet';
        link.setAttribute("data-status", "loading");
        // Add link to document body
        document.body.appendChild(link);
        // Store status in attribute on link
        // This can be read by other instances of this hook
        const setAttributeFromEvent = (event) => {
          link.setAttribute(
            "data-status",
            event.type === "load" ? "ready" : "error"
          );
        };
        link.addEventListener("load", setAttributeFromEvent);
        link.addEventListener("error", setAttributeFromEvent);
      } else {
        // Grab existing link status from attribute and set to state.
        setStatus(link.getAttribute("data-status"));
      }
      // link event handler to update status in state
      // Note: Even if the link already exists we still need to add
      // event handlers to update the state for *this* hook instance.
      const setStateFromEvent = (event) => {
        setStatus(event.type === "load" ? "ready" : "error");
      };
      // Add event listeners
      link.addEventListener("load", setStateFromEvent);
      link.addEventListener("error", setStateFromEvent);
      // Remove event listeners on cleanup
      return () => {
        if (link) {
          link.removeEventListener("load", setStateFromEvent);
          link.removeEventListener("error", setStateFromEvent);
        }
      };
    },
    [src] // Only re-run effect if link src changes
  );
  return status;
}