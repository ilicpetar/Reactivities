import { useLayoutEffect, useState } from "react";
import { BrowserRouterProps, Router } from "react-router-dom";
import { BrowserHistory, createBrowserHistory } from "history";

interface Props extends BrowserRouterProps {
  history: BrowserHistory;
}

// const history = createBrowserHistory();

export const CustomRouter = ({
  basename,
  history,
  children,
  ...props
}: Props) => {
  const [state, setState] = useState({
    action: history.action,
    location: history.location,
  });
  useLayoutEffect(() => history.listen(setState), [history]);
  return (
    <Router
      {...props}
      navigator={history}
      location={state.location}
      navigationType={state.action}
      children={children}
      basename={basename}
    />
  );
};
